@echo OFF
set testSet=%1

set packagesPath=..\..\packages\
set binPath=..\bin\
set outputPath=%binPath%testreports\%testSet%\

@echo on
mkdir %outputPath%
%packagesPath%NUnit.Runners.2.6.2\tools\nunit-console /labels /out=%outputPath%TestResult.txt /xml=%outputPath%TestResult.xml %binPath%Example.Specs.dll /include:%testSet%
%packagesPath%SpecFlow.1.9.0\tools\specflow.exe nunitexecutionreport %binPath%Example.Specs.csproj /testOutput:%outputPath%TestResult.txt /xmlTestResult:%outputPath%TestResult.xml /out:%outputPath%%testSet%-TestResult.html