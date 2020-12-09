function displayConvo(chatId) {
    let chatDiv = document.getElementById("messagelist");
    chatDiv.innerHTML = "";
    ChatInbox[chatId].Messages.forEach(function(message){
        let span = document.createElement("span");
        span.classList.add('message_container');
        let content = document.createElement("p");
        content.classList.add("message_content");
        content.innerHTML = message.Content;
        span.appendChild(content);
        let timestamp = document.createElement("span");
        timestamp.classList.add("message_timestamp");
        timestamp.innerHTML = message.Timestamp;
        span.appendChild(timestamp);
        chatDiv.appendChild(span);
    });
}