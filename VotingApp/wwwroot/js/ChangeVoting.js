let voting_list = document.getElementById("votings_view");
let voting_NameField = document.getElementById("voting_name");

voting_list.addEventListener("change", () => {
    let options = voting_list.options;
    voting_NameField.value = options[voting_list.selectedIndex].innerHTML;
});