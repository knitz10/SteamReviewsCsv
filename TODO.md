# Expanding the project (hopefully, uncertain if will happen) to include user profiles, to note whether the user will or will not like the game. How?

- [x] "Clean up" the project, (partially) sticking to OOP rules, make it ready for the big change that hopefully will happen (and so that it's generally easier to read)
- [x] Implement better/proper argument support
- [x] Improve the output, clean it up, making it ready for the big change \[...], and in general
- [ ] ~~Add a new feature; taking in account the User's profile for analisys (with either ollama, ML.NET, or some online ai api(maybe all will be supported)) of the games they play (public profile will be required, obviously), hopefully using the public Steam endpoints(?) or whatever so no api token is needed(prolly not gonna be possible without api token but we'll see)~~
  - [ ]  ~~Implement data analisys~~
    - [ ] ~~Implement getting data from the user's profile; and their played games~~
    - [ ] ~~Implement getting the user's games' data, taking in account the games' tags, and user's playtime in those, as well as their review (if exists)~~
         <!-- there is an endpoint without api key !-->
    - [ ] ~~Implement ML.NET for user analisys based on their profile and the game's page, as well as the player reviews, taking in account these fields(and possibly more):~~
            
            NumGamesOwned	NumReviews	PlaytimeForever	PlaytimeLastTwoWeeks	PlaytimeAtReview VotesUp	VotesFunny	WeightedVoteScore		SteamPurchase	ReceivedForFree	Refunded	WrittenDuringEarlyAccess
    - [ ] ~~Implement the outputting, why the user might like the game and why they might not like it using points based on the gathered data, as well as just displaying some data comparing the user to other players.~~
  - [ ] ~~(if unoptimized) optimize the program, data gathering(api usage), data analisys by the ai.~~

  - [ ] ~~Implement other recommendations based on player reviews (maybe, prolly wont happen tho)~~
  - [ ] ~~Possibly make a GUI for the output using Avalonia, and if not a GUI then a graphic output in some way, so the users don't have to look at a damn spreadsheet(also uncertain)~~

## Scope decision

I decided not to add the extra user-analysis checks because they are ultimately outside the purpose of this tool. The program is intended for data analysis of suspicious reviews and related review patterns, rather than analyzing whether an individual user will like a game. Sorry not sorry, as much as I liked that idea, I just don't think it fits the program.