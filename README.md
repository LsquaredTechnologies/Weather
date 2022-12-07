# Weather Forecast

Based from an original idea from this repository: https://github.com/Guillaume-SE/Skymood.github.io.

But the application is created using C# for both the Backend and the Frontend (using Blazor WebAssembly)

## Documentation

### Weather Forecast

This app is published (at the time of writing: end of 2022) on
[Azure Web App](https://weather-lsquared.azurewebsites.net/).

#### Dependencies

The weather and forecast information are retrieve using [OpenWeatherMap](https://openweathermap.org/)

### Improvements

I plan to:

- [ ] optimize styles with SASS and transpile into CSS with gulp (or another tool?)
- [ ] create an OCI (docker) image and publish it on DockerHub to let anyone test it locally
- [ ] create a pipeline to build and deploy the application (with both Azure Pipelines and Github Actions?)
- [ ] deploy Azure infrastructure with terraform (in progress)

## Contributing

Do not hesitate to post comments or improvements to the code (fork or PullRequest).
