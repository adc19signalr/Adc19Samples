const connection = new signalR.HubConnectionBuilder()
    .withUrl("/adcHub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.on("EmpfangeNachricht", (msg) => {
    $('#signalr-message-panel').prepend($('<div />').text(msg));
});

$('#btn-broadcast').click(function () {
    var msg = $('#broadcast').val();
    connection.invoke("BroadcastMessage", msg).catch(err => console.error(err.toString()));
});

$('#btn-self-message').click(function () {
    var msg = $('#self-message').val();
    connection.invoke("SendToCaller", msg).catch(err => console.error(err.toString()));
});

$('#btn-others-message').click(function () {
    var msg = $('#others-message').val();
    connection.invoke("SendToOthers", msg).catch(err => console.error(err.toString()));
});

$('#btn-group-message').click(function () {
    var msg = $('#group-message').val();
    var group = $('#group-for-message').val();
    connection.invoke("SendToGroup", group, msg).catch(err => console.error(err.toString()));
});

$('#btn-group-add').click(function () {
    var group = $('#group-to-add').val();
    connection.invoke("AddUserToGroup", group).catch(err => console.error(err.toString()));
});

$('#btn-group-remove').click(function () {
    var group = $('#group-to-remove').val();
    connection.invoke("RemoveUserFromGroup", group).catch(err => console.error(err.toString()));
});

async function start() {
    try {
        await connection.start();
        console.log('---connected---');
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 6000);
    }
};

connection.onclose(async () => {
    await start();
});

start();