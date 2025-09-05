using System.Text;
using DesafioProjetoHospedagem.Models;

Console.OutputEncoding = Encoding.UTF8;

// Cria os modelos de hóspedes e cadastra na lista de hóspedes
List<Pessoa> hospedes = new List<Pessoa>();

Pessoa p1 = new Pessoa(nome: "Hóspede 1");
Pessoa p2 = new Pessoa(nome: "Hóspede 2");

hospedes.Add(p1);
hospedes.Add(p2);

// Cria a suíte
Suite suite = new Suite(tipoSuite: "Premium", capacidade: 3, valorDiaria: 80);

// Cria uma nova reserva, passando a suíte e os hóspedes
Reserva reserva = new Reserva(diasReservados: 5);
reserva.CadastrarSuite(suite);
reserva.CadastrarHospedes(hospedes);

// Exibe a quantidade de hóspedes e o valor da diária
Console.WriteLine($"Hóspedes: {reserva.ObterQuantidadeHospedes()}");
Console.WriteLine($"Valor diária: {reserva.CalcularValorDiaria()}");


Console.WriteLine("O programa se encerrou");



// Testes adicionais
Console.WriteLine("Pressione uma tecla para continuar");
Console.ReadLine();

List<Pessoa> hospedes2 = new List<Pessoa>();
Suite suite2 = new Suite(tipoSuite: "Premium Plus", capacidade: 5, valorDiaria: 100);
Suite suite3 = new Suite(tipoSuite: "Simples", capacidade: 2, valorDiaria: 50);
Suite suite4 = new Suite(tipoSuite: "Single", capacidade: 1, valorDiaria: 30);

List<Suite> suites = new List<Suite> { suite, suite2, suite3, suite4 };
List<Reserva> reservas = new List<Reserva> {reserva};


bool exibirMenu = true;
while (exibirMenu)
{
    Console.Clear();
    Console.WriteLine("Seja bem vindo ao sistema de hopspedagem!");

    // Escolha da Suite  
    Console.Clear();
    int escolhaSuite = escolherSuite();
    if (escolhaSuite < 0)
    {
        Console.WriteLine("Não há suítes disponiveis.");
        break;
    }

    bool blnAdicionarHospedes = true;
    Console.WriteLine("Adicione os Hospedes:");
    while (blnAdicionarHospedes)
    {
        adicionarHospede(hospedes2);
        Console.WriteLine("Deseja adicionar mais hospedes? S (sim) / N (Não)");

        if (Console.ReadLine().ToString().ToUpper().Equals("N"))
        {
            blnAdicionarHospedes = false;
        }
    }

    // Definir dias reservados
    int diasReservados = 0;
    Console.Clear();
    Console.WriteLine("Digite a quantidade de dias a reservar:");
    Int32.TryParse(Console.ReadLine(), out diasReservados);
    diasReservados = (diasReservados < 1) ? 1 : diasReservados;


    // Definir a reserva
    Reserva r = new Reserva(diasReservados: diasReservados);
    r.CadastrarSuite(suites[escolhaSuite]);
    try
    {
        r.CadastrarHospedes(hospedes2);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message, ex.InnerException);
        Console.WriteLine("Não foi possivel fazer a reserva");
        Console.WriteLine("Pressione uma tecla para continuar");
        Console.ReadLine();
        hospedes2.Clear();
        Console.Clear();
        Console.WriteLine("Deseja refazer a reserva? S (sim) / N (Não)");
        if (Console.ReadLine().ToString().ToUpper().Equals("N"))
        {
            exibirMenu = false;
        }
        continue;
    }


    // Exibe a quantidade de hóspedes e o valor da diária
    Console.WriteLine($"Hóspedes: {r.ObterQuantidadeHospedes()}");
    Console.WriteLine($"Valor diária: {r.CalcularValorDiaria()}");

    reservas.Add(r);

    Console.WriteLine("Deseja fazer mais reservas? S (sim) / N (Não)");
    if (Console.ReadLine().ToString().ToUpper().Equals("N"))
    {
        exibirMenu = false;
    }
    hospedes2.Clear();

}

Console.WriteLine("O programa se encerrou");


void adicionarHospede(List<Pessoa> hospedes2)
{
    string nome1 = "";
    Console.Clear();
    Console.WriteLine($"Digite o neme do Hospede: {hospedes2.Count() + 1};");

    nome1 = Console.ReadLine();
    Pessoa pessoa = new Pessoa(nome: nome1);

    hospedes2.Add(pessoa);
    Console.WriteLine(" Hospede adicionado.");

}

int escolherSuite()
{
    int escolhaSuite = -1;
    List<string> suitesIndisponiveis = new List<string>();
    while (escolhaSuite == -1)
    {
        Console.WriteLine($"Escolha o número da suíte:");
        foreach (Suite s in suites)
        {
            Console.WriteLine($"{suites.IndexOf(s) + 1} - Suíte {s.TipoSuite}");
        }

        Int32.TryParse(Console.ReadLine(), out escolhaSuite);
        if (escolhaSuite <= 0 || escolhaSuite > suites.Count())
        {
            Console.WriteLine($"Opção {escolhaSuite} inválida.");
        }
        if (suiteReservada(suites[escolhaSuite - 1]))
        {
            Console.Clear();
            Console.WriteLine($"A suíte {escolhaSuite} está indisponível.");
            if (!suitesIndisponiveis.Contains(suites[escolhaSuite - 1].TipoSuite))
            {
                suitesIndisponiveis.Add(suites[escolhaSuite - 1].TipoSuite);
            }            
            if (suitesIndisponiveis.Count == 4) return -1;
            
            escolhaSuite = -1;            
        }

    }
    return escolhaSuite -1;
}


bool suiteReservada(Suite s)
{
    foreach (Reserva r in reservas)
    {
        if (r.Suite == s)
        {
            return true;
        }
        
    }
    return false;
}