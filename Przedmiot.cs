public class Przedmiot
{
    public int Id { get; set; } 
    public string Nazwa { get; set; }
    public string Lokalizacja { get; set; }
    public string Opis { get; set; }

    
    public Przedmiot(int id, string nazwa, string lokalizacja, string opis)
    {
        Id = id;
        Nazwa = nazwa;
        Lokalizacja = lokalizacja;
        Opis = opis;
    }
}
