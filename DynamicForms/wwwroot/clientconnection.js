

const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5026/FillForm")
    .configureLogging(signalR.LogLevel.Information)
    .build();


async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");

    } catch (err) {
        if (connection.state == "Disconnected") {
            console.log(err);
            setTimeout(start, 5000);
        }
    }
};

connection.onclose(async () => {
    await start();
});

connection.on("RecieveConnID", function (connid) {
    Connect.innerHTML = "ConnID: " + connid;
});

async function StartForm() {
    const formElement = document.getElementById("form_id");
    const progressElement = document.getElementById("progress_id");

    const formid = parseInt(formElement.value);
    const progressid = parseInt(progressElement.value);

    await connection.invoke("CheckForm", 1, 1);
}


connection.on("RecieveInput", function (Input) {
    addNewInput(Input);
});

connection.on("recieveformstatus", function (IsValid) {
    if (IsValid == false) {
        const InputLabel = document.getElementById("Inputs");
        InputLabel.innerHTML = "Form with submitted Id does not exist."
    }
});

connection.on("RecievePrice", function (Price) {
    const PriceLabel = document.getElementById("priceLabel");
    PriceLabel.innerHTML = "Price : " + Price;
}); 

// up to here we initialized the form

async function SendInputValue(InputId) {
    const InputValue = document.getElementById(InputId);
    var value =  InputValue.value;
    var req = {
        Id : parseInt(InputId),
        Value : value,
    };
    await connection.invoke("CheckInputValueValidity", req);
}

async function SubmitForm() {
    var Request = {
        RequestType: RequestType.SubmitForm,
    }
    await connection.invoke("SubmitFormRequest");
}

connection.on("RecieveSubmitCompletionStatus", function () {

});

async function NextStep() {
    await connection.invoke("NextStep");
}

async function previousStep() {
    await connection.invoke("PreviousStep");
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