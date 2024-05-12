let writing = (() => {
    let allowedToType = true;
    const setAllowedToType = (val) => {
        allowedToType = JSON.parse(val);
    }
    let activeRow = null;
    const tileClass = ".wordle__tile"
    const rowClass = ".wordle__row"

    const tryPlaceChar = char => {
        if (activeRow === null) {
            activeRow = getAvailableRow();
        }
        for (let tile of activeRow.querySelectorAll(tileClass)) {
            if (tile.textContent.trim() !== "") continue;
            tile.textContent = char.toUpperCase();
            return;
        }
    }
    const tryDeleteChar = () => {
        if (activeRow === null) return;
        let tiles = activeRow.querySelectorAll(tileClass);
        tiles = Array.from(tiles).reverse()
        for (let tile of tiles) {
            if (tile.textContent.trim() === "") continue;
            tile.textContent = "";
            return;
        }
    }
    const getAvailableRow = () => {
        if (activeRow !== null) return activeRow;
        const rows = document.querySelectorAll(rowClass)
        for (let row of rows) {
            if (!rowIsEmpty(row)) continue;
            return row
        }
    }
    const rowIsEmpty = row => {
        for (let tile of row.querySelectorAll(tileClass)) {
            if (tile.textContent.trim() !== "") {
                return false;
            }
        }
        return true;
    }
    const tryTakeGuess = () => {
        let guessInput = document.querySelector("#guessInput")
        guessInput.value = ExtractWordFromTiles();
        document.forms["guessForm"].requestSubmit();
        guessInput.value = ""
    }
    const ExtractWordFromTiles = () => {
        if (activeRow === null) return;
        let word = ""
        for (let tile of activeRow.querySelectorAll(tileClass)) {
            word += tile.textContent;
        }
        return word;
    }
    const addListener = () => {
        document.addEventListener("keydown", (event) => {
            if (!allowedToType) return;
            if (event.key.length === 1 && /[a-zA-Z]/.test(event.key)) {
                tryPlaceChar(event.key)
            } else if (["Backspace", "Delete"].includes(event.key)) {
                if (activeRow === null) return;
                tryDeleteChar()
            } else if (event.key === "Enter") {
                tryTakeGuess()
            }
        });
    }
    return {
        setAllowedToType,
        addListener
    }
})