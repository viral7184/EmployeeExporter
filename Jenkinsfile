pipeline {
    agent any
    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        SOLUTION_NAME = 'EmployeeExporter.sln'
    }
    stages {
        stage('Restore') {
            steps { sh 'dotnet restore ${SOLUTION_NAME}' }
        }
        stage('Build') {
            steps { sh 'dotnet build ${SOLUTION_NAME} --configuration Release --no-restore' }
        }
        stage('Test') {
            steps { sh 'dotnet test ${SOLUTION_NAME} --configuration Release --no-build' }
        }
        stage('Run') {
            steps {
                dir('EmployeeExporter.Worker') {
                    sh 'dotnet run --configuration Release --no-build'
                }
            }
        }
    }
    post {
        success {
            archiveArtifacts artifacts: 'EmployeeExporter.Worker/employees.csv', allowEmptyArchive: false
        }
    }
}
