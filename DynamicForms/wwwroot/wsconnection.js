var StartFormRequest;
var InputValueRequest;
var ValidityResponse;
var InputResponse;
var PriceResponse;
var InputInvalidResponse;


protobuf.load("form.proto", function (err, root) {
    if (err)
        throw err;
    StartFormRequest = root.lookupType("formpackage.StartFormRequest");
    InputValueRequest = root.lookupType("formpackage.InputValueRequest");
    ValidityResponse = root.lookupType("formpackage.ValidityResponse");
    InputResponse = root.lookupType("formpackage.InputResponse");
    PriceResponse = root.lookupType("formpackage.PriceResponse");
    InputInvalidResponse = root.lookupType("formpackage.InputInvalidResponse");
});

socket = new WebSocket("ws://localhost:5026/ws");
socket.binaryType = "arraybuffer";
socket.onopen = start();

async function start() {
    try {
        const labelElement = document.getElementById("stateLabel");
        labelElement.value ="Websocket Connected.";
        const request = {Index: 429496729, TextValue: "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc, quis gravida magna mi a libero. Fusce vulputate eleifend sapien. Vestibulum purus quam, scelerisque ut, mollis sed, nonummy id, metus. Nullam accumsan lorem in dui. Cras ultricies mi eu turpis hendrerit fringilla. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; In ac dui quis mi consectetuer lacinia. Nam pretium turpis et arcu. Duis arcu tortor, suscipit eget, imperdiet nec, imperdiet iaculis, ipsum. Sed aliquam ultrices mauris. Integer ante arcu, accumsan a, consectetuer eget, posuere ut, mauris. Praesent adipiscing. Phasellus ullamcorper ipsum rutrum nunc. Nunc nonummy metus. Vestibulum volutpat pretium libero. Cras id dui. Aenean ut eros et nisl sagittis vestibulum. Nullam nulla eros, ultricies sit amet, nonummy id, imperdiet feugiat, pede. Sed lectus. Donec mollis hendrerit risus. Phasellus nec sem in justo pellentesque facilisis. Etiam imperdiet imperdiet orci. Nunc nec neque. Phasellus leo dolor, tempus non, auctor et, hendrerit quis, nisi. Curabitur ligula sapien, tincidunt non, euismod vitae, posuere imperdiet, leo. Maecenas malesuada. Praesent congue erat at massa. Sed cursus turpis vitae tortor. Donec posuere vulputate arcu. Phasellus accumsan cursus velit. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Sed aliquam, nisi quis porttitor congue, elit erat euismod orci, ac placerat dolor lectus quis orci. Phasellus consectetuer vestibulum elit. Aenean tellus metus, bibendum sed, posuere ac, mattis non, nunc. Vestibulum fringilla pede sit amet augue. In turpis. Pellentesque posuere. Praesent turpis. Aenean posuere, tortor sed cursus feugiat, nunc augue blandit nunc, eu sollicitudin urna dolor sagittis lacus. Donec elit libero, sodales nec, volutpat a, suscipit non, turpis. Nullam sagittis. Suspendisse pulvinar, augue ac venenatis condimentum, sem libero volutpat nibh, nec pellentesque velit pede quis nunc. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Fusce id purus. Ut varius tincidunt libero. Phasellus dolor. Maecenas vestibulum mollis\n" +
                "\n"};
        const message = InputValueRequest.create(request);
        const buffer = InputValueRequest.encode(message).finish();

        const method = new Uint8Array([2]);

        var mergedArray = new Uint8Array(1 + buffer.length);
        mergedArray.set(method);
        mergedArray.set(buffer, 1);

        socket.send(mergedArray);
    } catch (err) {
        setTimeout(start, 5000);
    }
}

async function StartForm() {
    const formElement = document.getElementById("form_id");
    const progressElement = document.getElementById("progress_id");

    const request = {FormId: formElement.value, ProgressId: progressElement.value};
    const message = StartFormRequest.create(request);
    const buffer = StartFormRequest.encode(message).finish();
    
    const method = new Uint8Array([1]);

    var mergedArray = new Uint8Array(1 + buffer.length);
    mergedArray.set(method);
    mergedArray.set(buffer, 1);

    socket.send(mergedArray);
}

socket.onmessage = function (event) {
    const array = new Uint8Array(event.data);
    const ResponseType = array[0];
    const data = array.slice(1);
    switch (ResponseType) {
        case 1:
        {
            ShowMessage(data);
            return;
        }
        case 2:
        {
            ChangeStep(data);
            return;
        }
        case 3:
        {
            ShowInput(data);
            return;
        }
        case 4:
        {
            ViewPrice(data);
            return;
        }
        case 5:
        {
            ShowError(data);
            return;
        }
    }
    
}

// up to here we initialized the form
async function SendInputValue(InputId) {
    const Input = document.getElementById(InputId);
    var InputType = Input.type;
   
    const request = {Index : InputId};
    if (InputType == "text") {
        request.InputType = 0;
        request.NumValue = Input.value;
    } else if (InputType == "Integer") {
        request.NumValue = Input.value;
        request.InputType = 1;
    } else if (InputType == "Float") {
        request.NumValue = Input.value;
        request.InputType = 2;
    } else if (InputType == "Options") {
        request.NumValue = Input.value;
        request.InputType = 3;
    }

    const message = InputValueRequest.create(request);
    const buffer = InputValueRequest.encode(message).finish();

    const method = new Uint8Array([2]);

    var mergedArray = new Uint8Array(1 + buffer.length);
    mergedArray.set(method);
    mergedArray.set(buffer, 1);

    socket.send(mergedArray);
}

async function ViewPrice(data) {
    const Price = PriceResponse.decode(data);
    const PriceLabel = document.getElementById("priceLabel");
    PriceLabel.innerHTML = "Price : " + Price.Price;
}

async function ShowMessage(data) {
    const Response = ValidityResponse.decode(data);
    const MessageLabel = document.getElementById("messageLabel");
    MessageLabel.innerHTML = "Success: "+ Response.Valid+ "   Message: " + Response.message;
}

async function ChangeStep(data) {
    const Response = ValidityResponse.decode(data);
    if (Response.valid) {
        var inputsContainer = document.getElementById("inputs_list");
        inputsContainer.empty();
    }
    const MessageLabel = document.getElementById("messageLabel");
    MessageLabel.innerHTML = "Message : " + Response.message;
}

function ShowError(data) {
    const Error = InputInvalidResponse.decode(data);
    var errorsContainer = document.getElementById("errors_list");
    var label = document.createElement("label");
    label.textContent = "Input with Index:"+ Error.Index + " cannot have value " + Error.NumValue + ". must be " + Error.ErrorType + " than " + Error.ErrorValue;
    
    var br = document.createElement("br");
    errorsContainer.appendChild(label);
    errorsContainer.appendChild(br);
} 
function ShowInput(data) {
    const Input = InputResponse.decode(data);
    var inputsContainer = document.getElementById("inputs_list");
    var label = document.createElement("label");
    var typelabel = document.createElement("InputType");
    label.textContent = Input.Label + ":" + Input.Index;
    var input = document.createElement("input");
    input.id = Input.Index;
    if (Input.InputType == 0) {
        input.type = "Text";
    } else if (Input.InputType == 1) {
        input.type = "Integer";
    } else if (Input.InputType == 2) {
        input.type = "Float";
    } else if (Input.InputType == 3) {
        input.type = "Options";
    }
    input.value = Input.Placeholder;
    input.required = false;
    var button = document.createElement("button");
    button.id = Input.Index;
    button.setAttribute("onclick", "SendInputValue(this.id)");
    button.textContent = "Submit Value";
    var br = document.createElement("br");

    inputsContainer.appendChild(label);
    inputsContainer.appendChild(input);
    inputsContainer.appendChild(button);
    inputsContainer.appendChild(br);
}


async function NextStep() {
    const Request = new Uint8Array([3]);
    socket.send(Request);
}

async function PreviousStep() {
    const Request = new Uint8Array([4]);
    socket.send(Request);
}

async function SubmitForm() {
    const Request = new Uint8Array([5]);
    socket.send(Request);
}

async function Disconnect() {
    WebSocket.close();
}