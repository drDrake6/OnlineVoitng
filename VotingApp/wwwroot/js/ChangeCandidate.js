let candidate_list = document.getElementById("select_candidate");
let candidate_NameField = document.getElementById("candidate_name");

candidate_list.onchange = () => {
    let options = candidate_list.options;
    candidate_NameField.value = options[candidate_list.selectedIndex].innerHTML;
};