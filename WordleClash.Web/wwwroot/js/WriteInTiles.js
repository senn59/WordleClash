let activeRow = null;
document.addEventListener("keydown", (event) => {
    if (event.key.length === 1 && /[a-zA-Z]/.test(event.key)) {
        TryPlaceChar(event.key)
    }else if (["Backspace", "Delete"].includes(event.key)) {
        if (activeRow === null) return;
        DeleteChar()
    } else if (event.key === "Enter") {
        console.log("Guess")
    }
});

const TryPlaceChar = char => {
    if (activeRow === null) {
        activeRow = GetAvailableRow();
    }
    for (let tile of activeRow.querySelectorAll(".wordle-tile")) {
        if (tile.textContent.trim() !== "") continue;
        tile.textContent = char.toUpperCase();
        return;
    }
}
const DeleteChar = () => {
    if (activeRow === null) return;
    let tiles = activeRow.querySelectorAll(".wordle-tile");
    tiles = Array.from(tiles).reverse()
    for (let tile of tiles) {
        if (tile.textContent.trim() === "") continue;
        tile.textContent = "";
        return;
    }
}
const GetAvailableRow = () => {
    if (activeRow !== null) return activeRow;
    const rows = document.querySelectorAll(".wordle-row")
    for (let row of rows) {
        if (!RowIsEmpty(row)) continue;
        return row
    }
}
const RowIsEmpty = row => {
    for (let tile of row.querySelectorAll(".wordle-tile")) {
        if (tile.textContent.trim() !== "") {
            return false;
        }
    }
    return true;
}