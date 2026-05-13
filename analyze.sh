#!/bin/bash

# Configuration
SONAR_PROJECT_KEY="EShoppingZone_Backend"
SONAR_HOST_URL="http://localhost:9000"
# You should generate a token in SonarQube (My Account > Security > Tokens)
# And pass it as an argument or set it here.
SONAR_TOKEN="$1"

if [ -z "$SONAR_TOKEN" ]; then
    echo "Usage: ./analyze.sh <SONAR_TOKEN>"
    echo "Please provide the SonarQube analysis token."
    exit 1
fi

# Ensure the scanner path is in the PATH
export PATH="$PATH:$HOME/.dotnet/tools"

echo "Starting SonarQube analysis for EShoppingZone..."

dotnet-sonarscanner begin /k:"$SONAR_PROJECT_KEY" \
  /d:sonar.host.url="$SONAR_HOST_URL" \
  /d:sonar.login="$SONAR_TOKEN" \
  /d:sonar.exclusions="**/bin/**,**/obj/**,**/Migrations/**,**/logs/**,**/EShoppingZone-Frontend/**,**/EShoppingZone-Frontend-New/**" \
  /d:sonar.cs.opencover.reportsPaths="**/TestResults/**/coverage.opencover.xml"

# Build the project
dotnet build EShoppingZone.slnx --no-incremental

# End the analysis
dotnet-sonarscanner end /d:sonar.login="$SONAR_TOKEN"

echo "Analysis complete. Check your SonarQube dashboard at $SONAR_HOST_URL"
