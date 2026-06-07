# SteamReviewsCsv

This tool allows for easy downloading reviews from Steam using the https://store.steampowered.com/appreviews/ endpoint. 

You can customize the Url's filters and have a custom csv output if you wish to. More on that further down.

## Usage

To run this program, open your Cmd/Terminal, because this is a console application, and run ```path/to/program/binary``` (for example ```C:/Users/{YourUsername}/Downloads/SteamReviewCsv``` on Windows)

**This program supports these arguments:**
<sup><br>Warning! They need to be used in the mentioned order because I was too lazy to make them dynamic. You don't need to use all of them, but you have to make sure the order is correct. Help cannot be used with other arguments. Example of correct usage: ```SteamReviewCsv.exe 440 true ID,PersonaName,ReviewText recent,english,positive,non_steam_purchase,10,30,2,0</sup>
- ```help``` - shows general help
- ```appId``` - number, for example: 440. It is the appID of the game/app on Steam you'd want to get the reviews of. You can find it in the url:<br>[https://store.steampowered.com/app/**440**/Team_Fortress_2/](https://store.steampowered.com/app/440/Team_Fortress_2/)
- ```useRecommendedOutput``` - where to use the recommended (by me) output or not. false(default)/true.
- ```customOutput``` - when used correctly, the app will generate an additional {appId}_reviews_CustomOutput.csv, which will have whatever values you set. Comma separated(example: ID,PersonaName,ReviewText) Available values:
        <details>
        <summary>Review</summary>
            - ID<br>
            - RecommendationId<br>
            - RecommendationUrl<br>
            - Language<br>
            - ReviewText<br>
            - TimestampCreated<br>
            - DateCreated<br>
            - TimestampUpdated<br>
            - DateUpdated<br>
            - VotedUp<br>
            - Vote<br>
            - VotesUp<br>
            - VotesFunny<br>
            - WeightedVoteScore<br>
            - CommentCount<br>
            - SteamPurchase<br>
            - ReceivedForFree<br>
            - Refunded<br>
            - WrittenDuringEarlyAccess<br>
            - PrimarilySteamDeck<br>
            - AppReleaseDate<br>
            - AppReleaseDateTime<br>
    </details>
    <details>
    <summary>Author</summary>
            - ReviewID<br>
            - SteamId<br>
            - PersonaName<br>
            - PersonaStatus<br>
            - ProfileUrl<br>
            - NumGamesOwned<br>
            - NumReviews<br>
            - PlaytimeForever<br>
            - PlaytimeLastTwoWeeks<br>
            - PlaytimeAtReview<br>
            - LastPlayed<br>
            - LastPlayedDateTime<br>
            - Avatar<br>
            - FullAvatar<br>
    </details>
    <details>
    <summary>Hardware</summary>
            - Manufacturer<br>
            - Model<br>
            - DxVideoCard<br>
            - DxVendorId<br>
            - DxDeviceId<br>
            - NumGpu<br>
            - SystemRam<br>
            - Os<br>
            - CpuVendor<br>
            - CpuName<br>
            - GamingDeviceType<br>
            - DxDriverVersion<br>
            - AdapterDescription<br>
            - DriverVersion<br>
            - DriverDateRaw<br>
            - DriverDate<br>
            - VramSize<br>
            - ScreenWidth<br>
            - ScreenHeight<br>
            - PreciseFrameRate<br>
    </details>
- ```customFilters``` - when used correctly(parameter by parameter(can be null)), the app will use customized filters in the URL. Not recommended to use, and the param "all" for filter is unsupported, but if you really need to, here is an example: ```recent,english,positive,non_steam_purchase,10,30,2,0```. It maps to ```filter,language,review_type,purchase_type,num_per_page,day_range,start_offset,filter_offtopic_activity```. To find out more about these, look at [Steam's docs](https://partner.steamgames.com/doc/store/getreviews#:~:text=the%20parameters%20below.-,Parameters%3A,-GET%20store.steampowered).

## How to build

Run ```dotnet build``` in the project directory.