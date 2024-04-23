dotnet test -p:CollectCoverage=true -p:CoverletOutputFormat=cobertura
reportgenerator -reports:Tests/coverage.cobertura.xml  -targetdir:html-coverage-report/ -license:$(cat .env/ReportGeneratorLicense.txt)
cd html-coverage-report
explorer index.html
cd ..