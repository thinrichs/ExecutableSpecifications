mkdir %~dp0bin\testreports
%~dp0..\packages\SpecFlow.1.9.0\tools\specflow.exe stepdefinitionreport %~dp0\Example.Specs.csproj /binFolder:%~dp0bin /out:%~dp0bin\testreports\StepDefinitionReport.html