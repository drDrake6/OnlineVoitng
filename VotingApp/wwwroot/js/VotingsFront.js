async function GetResult() {
    let id = document.getElementById("votings_view").value;
    // отправляет запрос и получаем ответ
    const response = await fetch("/api/result/" + id, {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
    // если запрос прошел нормально
    if (response.ok === true) {
        // получаем данные
        const results = await response.json();
        let table = document.querySelector("tbody");
        ResetTable(table);
        
        if (document.title === "ChangeVoting - VotingApp")
        {
            results.forEach(result => {
                // добавляем полученные элементы в таблицу
                table.append(edit_row(result));               
            });
            table.append(await GenAddRow());
        }
        else if (document.title === "Vote - VotingApp")
        {
            results.forEach(result => {
                // добавляем полученные элементы в таблицу
                table.append(vote_row(result));
            });
        }
        else
        {
            results.forEach(result => {
                // добавляем полученные элементы в таблицу
                table.append(row(result));
            });
        }
    }
}

function ResetTable(table) {
    let rows = table.children;
    while (rows.length != 0) {
        rows[0].remove();
    }
}

function row(result) {

    const tr = document.createElement("tr");
    tr.setAttribute("data-rowid", result.id);

    const candidateTd = document.createElement("td");
    candidateTd.append(result.candidate.name);
    tr.append(candidateTd);

    const votesTd = document.createElement("td");
    votesTd.append(result.votes);
    tr.append(votesTd);

    return tr;
}

async function changeVoting(Votingid, VotingName, reset_votes) {
    
    const voting_response = await fetch("/api/voting", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            id: parseInt(Votingid, 10),
            name: VotingName,
            reset: reset_votes
        })
    });

    if (voting_response.ok === true) {
        location.reload();
    }
}

function edit_row(result) {

    const tr = document.createElement("tr");
    tr.setAttribute("data-rowid", result.id);

    const candidateTd = document.createElement("td");
    candidateTd.append(result.candidate.name);
    tr.append(candidateTd);

    const votesTd = document.createElement("td");
    votesTd.append(result.votes);
    tr.append(votesTd);

    const butTd = document.createElement("td");
    const deleteBut = document.createElement("input");
    deleteBut.setAttribute('type', 'button');
    deleteBut.classList.add('btn', 'btn-primary');
    deleteBut.setAttribute('value', 'Удалить');
    deleteBut.addEventListener("click", function () {
        DeleteResult(result.id);
    });
    butTd.append(deleteBut);
    tr.append(butTd);

    return tr;
}

async function AddResult(CandId) {

    const response = await fetch("/api/result", {
        method: "POST",
        headers: { "Accept": "application/json", "Content-Type": "application/json" },
        body: JSON.stringify({
            id: CandId,
            votingid: document.getElementById("votings_view").value
        })
    });
    if (response.ok === true) {
        const result = await response.json();
        document.getElementById("ResultCreator").before(edit_row(result));
    }
}

async function DeleteResult(id) {
    const response = await fetch("/api/result/" + id, {
        method: "DELETE",
        headers: { "Accept": "application/json" }
    });
    if (response.ok === true) {
        const result = await response.json();
        document.querySelector("tr[data-rowid='" + result.id + "']").remove();
    }
}

async function GenAddRow() {
    const tr = document.createElement("tr");
    tr.setAttribute('id', 'ResultCreator');

    const candidateTd = document.createElement("td");

    const choseCand = document.createElement("select");
    //choseCand.setAttribute("id", "ChoseCand");
    const response = await fetch("/api/candidate", {
        method: "GET",
        headers: { "Accept": "application/json" }
    });
     //если запрос прошел нормально
    if (response.ok === true) {
        // получаем данные
        const candidates = await response.json();
        candidates.forEach(candidate => {
            const optionCand = document.createElement("option");
            optionCand.setAttribute("value", candidate.id);
            optionCand.innerHTML = candidate.name;
            choseCand.append(optionCand);
        });
    }
    candidateTd.append(choseCand);

    tr.append(candidateTd);

    const butTd = document.createElement("td");
    const addBut = document.createElement("input");
    addBut.setAttribute('type', 'button');
    addBut.classList.add('btn', 'btn-primary');
    addBut.setAttribute('value', 'Добавить');
    addBut.setAttribute('id', 'AddNewResult');
    addBut.addEventListener("click", () => {
        AddResult(choseCand.value);
    })
    butTd.append(addBut);
    tr.append(butTd);

    return tr;
}

function vote_row(result) {
    const tr = document.createElement("tr");
    tr.setAttribute("data-rowid", result.id);

    const candidateTd = document.createElement("td");
    candidateTd.append(result.candidate.name);
    tr.append(candidateTd);

    const votesTd = document.createElement("td");
    votesTd.append(result.votes);
    tr.append(votesTd);

    const make_voteTd = document.createElement("td");
    const check = document.createElement("input");
    check.classList.add("form-check-input");
    check.setAttribute("type", "radio");
    check.setAttribute("name", "common");
    make_voteTd.append(check);
    tr.append(make_voteTd);

    return tr;
}

async function MakeVote() {
    for (let el of document.querySelector("tbody").children) {
        if (el.querySelector("input").checked) {
            const vote_response = await fetch("/api/result/" + el.getAttribute("data-rowid"), {
                method: "POST",
                headers: { "Accept": "application/json" }
            });
            if (vote_response.ok === true) {
                location.reload();
            }
            else {
                //overvote
                const errorData = await vote_response.json();
                console.log(errorData["overvote"]);
                if (errorData) {
                    // ошибки вследствие валидации по атрибутам
                    if (errorData.overvote) {
                        addError(errorData.overvote);
                    }
                }
            }
            return;
        }            
    }
    alert("нужно отдать голос");
}

function addError(errors) {
    errors.forEach(error => {
        const p = document.createElement("p");
        p.classList.add("alert-error");
        p.append(error);
        document.getElementById("errors").append(p);
    });
}

function ChangeVotingWrap() {
    changeVoting(
        document.getElementById("votings_view").value,
        document.getElementById("voting_name").value,
        document.getElementById("resetVotes").checked);
}


document.getElementById("votings_view").addEventListener("change", GetResult);

if (document.getElementById("voting_change") != null)
    document.getElementById("voting_change").addEventListener("click", ChangeVotingWrap);

if (document.getElementById("make_vote_btn") != null)
    document.getElementById("make_vote_btn").addEventListener("click", MakeVote);

