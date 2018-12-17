@echo off



SET TestResultsFileProjectName=PerformanceReport

SET DLLToTestRelativePath=TaskManagerApi.Tests\bin\Debug\TaskManagerApi.Tests.dll

SET NBenchRunnerFolderName=NBench.Runner.1.2.2



if not exist "%~dp0PerformanceReports" mkdir "%~dp0PerformanceReports"



REM Remove any previously created test output directories

CD %~dp0

FOR /D /R %%X IN (%USERNAME%*) DO RD /S /Q "%%X"





call :RunNBench



:RunNBench

"%~dp0packages\%NBenchRunnerFolderName%\tools\net452\NBench.Runner.exe" ^

%~dp0%DLLToTestRelativePath% ^

--output "%~dp0PerformanceReports"

REM NBench.Runner.exe %~dp0%DLLToTestRelativePath% --output:"%~dp0PerformanceReports"



exit /b %errorlevel%