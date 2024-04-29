let activeRow = null;
const tileClass = ".wordle__tile"
const rowClass = ".wordle__row"
document.addEventListener("keydown", (event) => {
    if (event.key.length === 1 && /[a-zA-Z]/.test(event.key)) {
        TryPlaceChar(event.key)
    } else if (["Backspace", "Delete"].includes(event.key)) {
        if (activeRow === null) return;
        TryDeleteChar()
    } else if (event.key === "Enter") {
        TryTakeGuess()
    }
});

const TryPlaceChar = char => {
    if (activeRow === null) {
        activeRow = GetAvailableRow();
    }
    for (let tile of activeRow.querySelectorAll(tileClass)) {
        if (tile.textContent.trim() !== "") continue;
        tile.textContent = char.toUpperCase();
        return;
    }
}
const TryDeleteChar = () => {
    if (activeRow === null) return;
    let tiles = activeRow.querySelectorAll(tileClass);
    tiles = Array.from(tiles).reverse()
    for (let tile of tiles) {
        if (tile.textContent.trim() === "") continue;
        tile.textContent = "";
        return;
    }
}
const GetAvailableRow = () => {
    if (activeRow !== null) return activeRow;
    const rows = document.querySelectorAll(rowClass)
    for (let row of rows) {
        if (!RowIsEmpty(row)) continue;
        return row
    }
}
const RowIsEmpty = row => {
    for (let tile of row.querySelectorAll(tileClass)) {
        if (tile.textContent.trim() !== "") {
            return false;
        }
    }
    return true;
}
const TryTakeGuess = () => {
    document.querySelector("#guessInput").value = ExtractWordFromTiles();
    document.forms["guessForm"].submit();
}
const ExtractWordFromTiles = () => {
    if (activeRow === null) return;
    let word = ""
    for (let tile of activeRow.querySelectorAll(tileClass)) {
        word += tile.textContent;
    }
    return word;
}