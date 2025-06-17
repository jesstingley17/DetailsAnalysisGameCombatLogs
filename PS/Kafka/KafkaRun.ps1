$serverScriptPath = "F:\Kafka\bin\windows"
$propertyPath = "F:\Kafka\config"

cmd.exe /c "$serverScriptPath\kafka-server-start.bat $propertyPath\server.properties"