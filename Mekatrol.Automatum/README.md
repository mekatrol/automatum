`dotnet new tool-manifest`
`dotnet tool install --global Swashbuckle.AspNetCore.Cli`
`dotnet tool restore`
`npm i swagger-typescript-api`

```bash
pushd ../../automatum/Mekatrol.Automatum/Mekatrol.Automatum.NodeServer
dotnet msbuild -t:CreateSwaggerJson
popd
```

```bash
swagger-typescript-api -p ../../automatum/Mekatrol.Automatum/Mekatrol.Automatum.NodeServer/swagger.json --axios -o ./src/services -n api-generated.ts --unwrap-response-data --templates src/services/api-templates
```