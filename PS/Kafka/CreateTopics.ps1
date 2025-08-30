$serverScriptPath = "F:\Kafka\bin\windows"
$topics = @("personal-chat", "personal-chat-message", "group-chat", "group-chat-message", "group-chat-unread-message", "group-chat-member", "notification")

foreach ($topic in $topics) {
    cmd.exe /c "$serverScriptPath\kafka-topics.bat --create --bootstrap-server localhost:9092 --replication-factor 1 --partitions 1 --topic $topic"
}