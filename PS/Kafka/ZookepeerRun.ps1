$serverScriptPath = "F:\Kafka\bin\windows"
$propertyPath = "F:\Kafka\config"

cmd.exe /c "$serverScriptPath\zookeeper-server-start.bat $propertyPath\zookeeper.properties"