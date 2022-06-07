using System;
using System.Collections.Generic;
					
public class Program
{
	public static void Main()
	{	
		String searchTerm = "banG";
		
		CitySearch.CityFinder myFinder = new CitySearch.CityFinder();
		CitySearch.ICityResult myResults = myFinder.Search(searchTerm);
		
		Console.WriteLine("Search Results for: `" + searchTerm + "`");
		Console.WriteLine("\nNextCities Result:" + myResults.NextCities.Count);

		foreach (string cityName in  myResults.NextCities) {
			Console.WriteLine("`" + cityName + "`");
		}
		Console.WriteLine("\nNextLetters Result:" + myResults.NextLetters.Count);
        foreach (string letter in  myResults.NextLetters) {
			Console.WriteLine("`" + letter + "`");
		}
	}
}

namespace CitySearch
{
    using System.Collections.Generic;

    public interface ICityResult
    {
        ICollection<string> NextLetters { get; set; }
        ICollection<string> NextCities { get; set; }
    }
	
	public class CityResult : ICityResult
	{
		public ICollection<string> NextLetters { get; set; }
		public ICollection<string> NextCities { get; set; }
		
		public CityResult() {
			NextLetters = new HashSet<String>();
			NextCities = new HashSet<String>();
		}
	}
	
	public interface ICityFinder
    {
        ICityResult Search(string searchString);
    }
	
	public class CityFinder : ICityFinder 
	{			
		string[] testData = {
			"LAGOS", 
            "LA PAZ", 
            "LA PLATA",
			"LA-EPLATA",
			"LEEDS",
			"BANDUNG",
			"BANGUI",
			"BANGKOK",
			"BANGKOK",
			"BANGALORE",
			"ZARIA",
			"ZHUGHAI",
			"ZIBO"			
		};
			
		HashSet<String> sourceData;
	
		public CityFinder() {
		    // initialize source data
		    sourceData = new HashSet<String>(testData);
		}
		
		
		public ICityResult Search(string searchString)
		{
		    // normalize search string to lowercase
		    String searchStringNormalized = searchString.ToLower();
			int nexCharPosition = searchString.Length;
			CitySearch.ICityResult searchResults = new CitySearch.CityResult();
			
			foreach (string cityName in sourceData)
			{
			    // normalize data to lowercase
				if (cityName.ToLower().StartsWith(searchStringNormalized)) {
				   // add to city results
				   searchResults.NextCities.Add(cityName);
				   searchResults.NextLetters.Add(cityName[nexCharPosition].ToString());
				}
			}
			
			return searchResults;
		}
	}
}
