# Project Title

## Generating Test Coverage

This requires ReportGenerator: [LINK](https://github.com/danielpalme/ReportGenerator)

```
dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura
reportgenerator -reports:Tests/coverage.cobertura.xml  -targetdir:html-coverage-report/
```