// Mascaramento ao vivo (dígito por dígito) para telefone e CPF/CNPJ.
// Roda 100% no cliente, direto no evento "input" do <input> nativo — não usa o MudMask do
// MudBlazor porque ele tem bug conhecido de cursor pulando em Blazor Server (só funciona bem em
// WASM). Como isso mexe no DOM antes do round-trip do Blazor Server terminar, quando a resposta do
// servidor chega o valor já bate e o Blazor não sobrescreve o input nem reseta o cursor.
window.mascaras = (function () {
    function digitosDe(valor) {
        return (valor || "").replace(/\D/g, "");
    }

    function formatarTelefone(digitosCompletos) {
        const d = digitosCompletos.slice(0, 11);
        if (d.length === 0) return "";
        if (d.length <= 2) return `(${d}`;
        const ddd = d.slice(0, 2);
        const resto = d.slice(2);
        if (d.length <= 10)
            return resto.length <= 4 ? `(${ddd}) ${resto}` : `(${ddd}) ${resto.slice(0, 4)}-${resto.slice(4)}`;
        return `(${ddd}) ${resto.slice(0, 5)}-${resto.slice(5)}`;
    }

    // Até 11 dígitos formata como CPF; a partir do 12º dígito passa a formatar como CNPJ.
    function formatarCpfCnpj(digitosCompletos) {
        if (digitosCompletos.length <= 11) {
            const d = digitosCompletos.slice(0, 11);
            if (d.length <= 3) return d;
            if (d.length <= 6) return `${d.slice(0, 3)}.${d.slice(3)}`;
            if (d.length <= 9) return `${d.slice(0, 3)}.${d.slice(3, 6)}.${d.slice(6)}`;
            return `${d.slice(0, 3)}.${d.slice(3, 6)}.${d.slice(6, 9)}-${d.slice(9)}`;
        }
        const d = digitosCompletos.slice(0, 14);
        if (d.length <= 2) return d;
        if (d.length <= 5) return `${d.slice(0, 2)}.${d.slice(2)}`;
        if (d.length <= 8) return `${d.slice(0, 2)}.${d.slice(2, 5)}.${d.slice(5)}`;
        if (d.length <= 12) return `${d.slice(0, 2)}.${d.slice(2, 5)}.${d.slice(5, 8)}/${d.slice(8)}`;
        return `${d.slice(0, 2)}.${d.slice(2, 5)}.${d.slice(5, 8)}/${d.slice(8, 12)}-${d.slice(12)}`;
    }

    // Mantém o cursor "andando junto" com os dígitos em vez de pular pro final a cada tecla.
    function posicaoAposDigitos(formatado, quantidadeDigitos) {
        if (quantidadeDigitos <= 0) return 0;
        let contados = 0;
        for (let i = 0; i < formatado.length; i++) {
            if (/\d/.test(formatado[i])) {
                contados++;
                if (contados === quantidadeDigitos) return i + 1;
            }
        }
        return formatado.length;
    }

    function aplicar(input, tipo) {
        const cursorAtual = input.selectionStart ?? input.value.length;
        const digitosAntesDoCursor = digitosDe(input.value.slice(0, cursorAtual)).length;
        const digitos = digitosDe(input.value);
        const formatado = tipo === "cpfcnpj" ? formatarCpfCnpj(digitos) : formatarTelefone(digitos);

        if (input.value === formatado) return;

        input.value = formatado;
        const novaPosicao = posicaoAposDigitos(formatado, digitosAntesDoCursor);
        input.setSelectionRange(novaPosicao, novaPosicao);
    }

    // "containerId" é o id de um elemento (div) em volta do MudTextField — buscamos o <input>
    // real dentro dele, já que o id gerado pelo próprio MudBlazor no input muda a cada render.
    function iniciar(containerId, tipo) {
        const container = document.getElementById(containerId);
        const input = container && container.querySelector("input");
        if (!input || input.dataset.mascaraAtiva) return;

        input.dataset.mascaraAtiva = "1";
        aplicar(input, tipo);
        input.addEventListener("input", () => aplicar(input, tipo));
    }

    return { iniciar };
})();
