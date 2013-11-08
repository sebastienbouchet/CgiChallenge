using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DTStrike.MyBot
{
    public class Program
    {
    	
        // The DoTurn function is where your code goes. The Game object
	    // contains the state of the game, including information about all planets
	    // and fleets that currently exist. Inside this function, you issue orders
	    // using the game.issueOrder() function. For example, to send 10 ships from
	    // planet 3 to planet 8, you would say game.issueOrder(3, 8, 10).
	    //
	    // There is already a basic strategy in place here. You can use it as a
	    // starting point, or you can throw it out entirely and replace it with
	    // your own.
	    public static void doTurn(Game game) {
	
		    // (1) Choix de la source : la plus forte
		    /*Planet source = null;
		    int sourceShips = int.MinValue;
		    foreach (Planet p in game.getMyMilitaryPlanets()) {
			    int score = p.numShips;
			    if (score > sourceShips) {
				    sourceShips = score;
				    source = p;
			    }
		    }

            if (source == null)
            {
                return;
            }*/

		    //	2	Choix de la cible : la plus faible et la plus proche de la source
			//	2.1	Soit indus soit mili
		    Planet dest = null;
		    Planet source = null;
		    double destScore = 0;
		    int destFleetShips = 0;
		    List<Planet> cibles = game.getNotMyIndusPlanets();
		    if ((cibles.Count == 0) || (game.getMyIndusPlanets().Count / game.getMyMilitaryPlanets().Count > 5)) {
		    	cibles = game.getNotMyMiliPlanets();
		    }
		    foreach (Planet p in cibles)
		    {
		    	foreach (Planet s in game.getMyMilitaryPlanets()) {
		    		int fleetShips = game.getShipsWithFleet(p) + 1;
		    		
		    		// taille mini de la flotte
			    	if(fleetShips < 10) {
			    		fleetShips = 10;
			    	}
		    		
		    		if ((fleetShips < s.numShips / 2) && (s.numShips - fleetShips > 10)) {
		    			
			    		double score = game.getDestScore(p,s);
			    		if(score > destScore) {
		                	dest = p;
		                	destScore = score;
		                	destFleetShips = fleetShips;
		                	source = s;
			    		}
			    		
		    		}
		    	}
		    }

            
            //Log.debug("dest: " + dest.id.ToString() + " source  " + source.id.ToString());

		    //	3	On envoie les fleet
		    if (source != null && dest != null) {
                game.issueOrder(source, dest, destFleetShips);
            }
            else
            {
                return;
            }
	    }

	    public static void Main(String[] args) {

		    try {
                String line;
                while ((line = System.Console.ReadLine()) != "ready")
                {
                    if (line.StartsWith("*"))
                    {
                        // this is an option
                    }
                }
                System.Console.Out.Write("go\n");
                System.Console.Out.Flush();

                while (true) 
                {
                    List<String> data = new List<string>();
                    while ((line = System.Console.ReadLine()) != "go")
                    {
                        data.Add(line);
                    }
                    Game game = new Game(data);
                    doTurn(game);
                    game.finishTurn();
                    System.Console.Out.Flush();
                }
		    } finally {
			
		    }
	    }
    }
}
