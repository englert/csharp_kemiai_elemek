/* felfedezesek.csv http://www.infojegyzet.hu/vizsgafeladatok/okj-programozas/szoftverfejleszto-elemek/
�v;Elem;Vegyjel;Rendsz�m;Felfedez�
�kor;Arany;Au;79;Ismeretlen
1250;Arz�n;As;33;Albertus Magnus

https://vslapp.files.wordpress.com/2011/11/linq-cheatsheet.pdf
*/
using System;                     // Console
using System.IO;                  // StreamReader
using System.Text;                // Encoding
using System.Collections.Generic; // List<>, Dictionary<>
using System.Linq;

class Kemia{
    public string ev        {get; set;}
    public string elem      {get; set;}
    public string vegyjel   {get; set;}
    public string rendszam  {get; set;}
    public string felfedezo {get; set;}

    public Kemia(string sor){
        var s = sor.Trim().Split(';');
        this.ev         = s[0];
        this.elem       = s[1];
        this.vegyjel    = s[2];
        this.rendszam   = s[3];
        this.felfedezo  = s[4];
    }
}
class Program {
    public static bool is_vegyjel(string sor){
        if (sor.Length > 2 || sor.Length < 1){
            return false;
        }
        var alfa = "ABCDEFGHIJKLMNOPQRSTUVwXYZ";
        foreach( var c in sor.ToUpper() ){
            if (!alfa.Contains(c)){
                return false;
            } 
        }
        return true;
    }
    public static void Main (string[] args) {
        var lista = new List<Kemia>();              
        var f =  new StreamReader("felfedezesek.csv", Encoding.GetEncoding("iso-8859-1")); 
        var elsosor = f.ReadLine(); 
        while (!f.EndOfStream){             
		    lista.Add(  new Kemia(f.ReadLine())  );                       
		}
        f.Close(); 
    
        // 3. feladat: Elemek száma: {}
        Console.WriteLine($"3. feladat: Elemek száma: {lista.Count}");

        // 4. feladat: Felfedezések száma az ókorban: {}
        var okor_db = (
            from sor in lista
            where sor.ev == "Ókor"
            select sor
            ).Count();
        Console.WriteLine($"4. feladat: Felfedezések száma az ókorban: {okor_db}");
    
        // 5. feladat: Kérek egy vegyjelet: {}
        var vegyjel = "";
        while(true){
            Console.Write($"5. feladat: Kérek egy vegyjelet: ");
            vegyjel = Console.ReadLine().ToUpper();
            if (is_vegyjel(vegyjel)){
                break;
            }
        }

        // 6. feladat: Keresés
        Console.WriteLine(     $"6. feladat: Keresés"); 
        var elem = (
            from sor in lista
            where sor.vegyjel.ToUpper() == vegyjel
            select sor
            );
        if (elem.Any()){
            var x = elem.First();
            Console.WriteLine( $"        Az elem vegyjele: {x.vegyjel}");        
            Console.WriteLine( $"        Az elem neve: {x.elem}");
            Console.WriteLine( $"        Rendszáma: {x.rendszam}");        
            Console.WriteLine( $"        Felfedezés éve: {x.ev}");        
            Console.WriteLine( $"        Felfedező: {x.felfedezo}");        
        }
        else{
            Console.WriteLine($"        Nincs ilyen elem az adatforrásban!"); 
        }

        // 7. feladat: {} év volt a leghosszabb időszak két elem felfedezése között.
        var evek = (
            from sor in lista
            where sor.ev != "Ókor"
            select int.Parse(sor.ev)
            ).ToList();
    
        var dif_lista = new List<int>();
        for(int i=0; i<evek.Count()-1; i++){
            dif_lista.Add(evek[i+1] - evek[i]);
        }
        Console.WriteLine($"7. feladat: {dif_lista.Max()} év volt a leghosszabb időszak két elem felfedezése között.");

        // 8. feladat: Statisztika
        var statisztika = (
            from sor in lista
            where sor.ev != "Ókor"
            group sor by sor.ev
            into res
            where res.Count() > 3
            select res 
            );

        Console.WriteLine(    $"7. feladat: Statisztika"); 
        foreach(var s in statisztika){
            Console.WriteLine($"        {s.Key}: {s.Count()} db");
        }
        //--------------------------------------------------------
    }
}