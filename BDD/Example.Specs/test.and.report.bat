REM @echo OFF
set testSet=%1

set packagesPath=..\..\packages\
set binPath=..\bin\
set outputPath=%binPath%testreports\%testSet%\

@echo on
mkdir %outputPath%
%packagesPath%xunit.runners.1.9.2\tools\xunit.console.clr4.exe  %binPath%Example.Specs.dll  /nunit %outputPath%TestResult.txt
%packagesPath%SpecFlow.1.9.0\tools\specflow.exe nunitexecutionreport %binPath%Example.Specs.csproj /testOutput:%outputPath%TestResult.txt /xmlTestResult:%outputPath%TestResult.xml /out:%outputPath%%testSet%-TestResult.html