namespace RummiKub.GamePlay
{
  public class Class1
  {

  }

  //goal empty all of your tiles
  //(done) tiles: 106
  //(done) 2 jokers wild
  //(misc.) racks: one per player

  //(done)each player gets 14 tiles

  //set: 3 or 4 same number all different colors e.g. 7d 7c 7h
  //run: sequence 3+ consecutive numbers all the same color 4c 5c 6c
  //no duplicates allowed

  //1st play is combo with 30 points or more
  //points - face value of tiles
  //jokers = the vlaue of the tile they stand for
  //only use tiles from your own rack for opening play

  //if you can't play draw one tile , end tyrn (pass)

  //your turn
  //lay down new sets/runs from your rack
  //add tukes ti existing sets/runs on table
  //rearrange the table
  //    split runs, extend sets, shift tiles around
  //    at the endof the turn everything must be a legal set

  //use a joker
  //  can represent any tile
  //  if a joker is on the table you can rpeplace it with actual tile and must immediately reuse the joker in same turn

  //if you can't or choose not to play draw 1 tile end turn (pass)
  //player goes out by playing their last tules
  //left tiles -. negative score
  //winner adds all opponents left totals to their score

  //if pool of tiles is empty and no one can play game ends and lowest tile total wins

  //scoring
  //tile values = face value
  //joker = 30 if on your rack
  //winner adds opponents score to their score
  //others subtract leftover totals
  //play until someone reachs 200-300 points

  //strategy
  //hold back early
  //dump big numbers (12, 13, joker) before someone goes out
  //rearrangements = power moves - scan for table shits
  //joker control - reclaim or holding one can swing game


  


}
