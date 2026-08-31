pipeline {
    agent any
    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        SOLUTION_NAME = 'EmployeeExporter.sln'
    }
    stages {
        stage('Restore') {
            steps { bat "dotnet restore ${SOLUTION_NAME}" }
        }
        stage('Build') {
            steps { bat "dotnet build ${SOLUTION_NAME} --configuration Release --no-restore" }
        }
        stage('Test') {
            steps { bat "dotnet test ${SOLUTION_NAME} --configuration Release --no-build" }
        }
        stage('Run') {
            steps {
                dir('EmployeeExporter.Worker') {
                    bat "dotnet run --configuration Release --no-build"
                }
            }
        }
    }
    post {
        success {
            archiveArtifacts artifacts: 'EmployeeExporter.Worker/Output/employees_*.csv', allowEmptyArchive: false
        }
    }
}