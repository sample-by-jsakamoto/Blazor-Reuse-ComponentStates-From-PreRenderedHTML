var slnDir = FindContainerDirToAncestor("BlazorApp1.sln");
var projectDir = FindContainerDirToAncestor("Demonstration.csproj");
var apiServerDir = Path.Combine(slnDir, "APIServer");
var blazorAppDir = Path.Combine(slnDir, "BlazorApp1");
var publishedDir = Path.Combine(blazorAppDir, "bin", "Release", "net6.0", "publish", "wwwroot");

WriteLine();
WriteLine(Cyan("Start the API server..."));
using var apiServerProc = Start("dotnet", "run", apiServerDir);
var apiServerLaunced = await apiServerProc.WaitForOutputAsync(output => output.Contains("Now listening on: https://localhost"), millsecondsTimeout: 10000);
WriteLine(DarkGray(apiServerProc.Output));
if (!apiServerLaunced) { WriteLine(Red($"ERROR: The API server could not be launced")); return; }

WriteLine();
WriteLine(Cyan("Publish the Blazor Wasm app..."));
using var publishProc = Start("dotnet", "publish -c Release", blazorAppDir);
await foreach (var outout in publishProc.GetOutputAsyncStream()) { WriteLine(DarkGray(outout)); }
if (publishProc.ExitCode != 0) { WriteLine(Red($"ERROR: The exit code of publishing process was not 0, it was {publishProc.ExitCode}.")); return; }

WriteLine();
WriteLine(Yellow("Stop the API server..."));
apiServerProc.Process.Kill();
await apiServerProc.WaitForExitAsync();

WriteLine();
WriteLine(Cyan("Start the static HTTP server on TCP port 5001 to serve the published contents..."));
using var restoreProc = await Start("dotnet", "tool restore", projectDir).WaitForExitAsync();
if (restoreProc.ExitCode != 0) { WriteLine(Red($"ERROR: The exit code of restoring tool process was not 0, it was {publishProc.ExitCode}.") + "\r\n" + DarkGray(restoreProc.Output)); return; }

using var httpServerProc = Start("dotnet", $"serve -d:\"{publishedDir}\" -S -p:5001 --default-extensions:html");
var httpServerLaunced = await httpServerProc.WaitForOutputAsync(output => output.Contains("https://localhost"), millsecondsTimeout: 10000);
WriteLine(DarkGray(httpServerProc.Output));
if (!httpServerLaunced) { WriteLine(Red($"ERROR: The static HTTP server could not be launced")); return; }


WriteLine();
WriteLine(Cyan("Reay to go!"));
WriteLine(Yellow("Please check that the \"Fetch Data\" page ") + Red("would not work ") + Yellow("because the AP server is not running."));
Process.Start(new ProcessStartInfo("https://localhost:5001/") { UseShellExecute = true });


WriteLine();
WriteLine(Cyan("Press ESC key to exit."));
while (ReadKey(intercept: true).Key != ConsoleKey.Escape) ;

WriteLine(Cyan("Complete."));
