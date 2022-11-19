# The demonstration of the Blazor WebAssembly app - reusing component states embedded in static prerendered HTML files

## Summary

This repository is for demonstrating a Blazor WebAssembly app that will show us how to resolve a proposition below. The .NET6 SDK is required if you run the app of this repository on your PC.

## Backgrounds

- This is the Blazor WebAssembly app project that will be prerendered for each page into static HTML files at its publishing.
- The app will fetch weather forecast data from the API server and render it.
- And also, the app is prerendered with the `WebAssemblyPrerendered` mode, and it uses `PersistentComponentState` to persist the weather forecast data fetched from the API server to skip a "Loading..." progress indicator.
- **However, the API endpoints are only available during the Blazor Wasm app is prerendering.** The API server has already been shut down at the time users are opening the app on a production site.

## Proposition

What can we do to render the weather forecast on the Blazor WebAssembly app hosted on a production server even if the API server is unavailable?

P.S.
There is no need to display precisely the latest weather forecast. It is enough that the weather forecast data at the publishing time can be shown.

## Solution

The weather forecast data at the publishing time can be found the inside the static prerendered HTML file because it was persisted by the  `PersistentComponentState.PersistAsJson()`.

So the app can fetch the static prerendered HTML file instead of the API server, parses it, and finally gets the weather forecast data at the publishing time.

For more detail, please see [the commit log](https://github.com/sample-by-jsakamoto/Blazor-Reuse-ComponentStates-From-PreRenderedHTML/commits/main) of this repository.

## Demonstration

You can see the solution above definitely work fine with the following command.

```
git clone git@github.com:sample-by-jsakamoto/Blazor-Reuse-ComponentStates-From-PreRenderedHTML.git .

dotnet run --project ./.demonstration/Demonstration.csproj
```

## License

[The Unlicense](LICENSE)