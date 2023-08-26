var AwesomeMessage;

protobuf.load("form.proto", function(err, root) {
    if (err)
        throw err;

    // Obtain a message type
     AwesomeMessage = root.lookupType("formpackage.Request");

});
socket = new WebSocket("ws://localhost:5026/ws");
socket.binaryType = "arraybuffer";
socket.onopen = start();

async function start() {
    try {
        console.log("SignalR Connected.");
        var request = { Id : 2, Value : 22}
        var message = AwesomeMessage.create(request);
        var buffer = AwesomeMessage.encode(message).finish();
     
        socket.send(buffer);
    } catch (err) {
        
            setTimeout(start, 5000);
        
    }
};


async function StartForm() {
    const formElement = document.getElementById("form_id");
    const progressElement = document.getElementById("progress_id");

    const formid = parseInt(formElement.value);
    const progressid = parseInt(progressElement.value);

}

// up to here we initialized the form

async function SendInputValue(InputId) {
    const InputValue = document.getElementById(InputId);
    var value =  InputValue.value;
    var req = {
        Id : parseInt(InputId),
        Value : value,
    };
}

async function SubmitForm() {
    var Request = {
        RequestType: RequestType.SubmitForm,
    }
}



function addNewInput(InputObject) {
    var Input = JSON.parse(InputObject);
    var inputsContainer = document.getElementById("inputs_list");
    var label = document.createElement("label");
    label.textContent = "Input " + InputObject + ":" + Input.Id;
    var input = document.createElement("input");
    input.id = Input.Id;
    input.type = "text";
    input.required = false;
    var button = document.createElement("button");
    button.id = Input.Id;
    button.setAttribute("onclick", "SendInputValue(this.id)");
    button.textContent = "Submit Value";
    var br = document.createElement("br");

    inputsContainer.appendChild(label);
    inputsContainer.appendChild(input);
    inputsContainer.appendChild(button);
    inputsContainer.appendChild(br);
}

start();