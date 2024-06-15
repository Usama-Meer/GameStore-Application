using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStore.Dtos;


namespace GameStore.Endpoints;

public static class GameEndpoints
{

    public const string GetGameEndpoint = "GetGames";
    
    private static readonly List<GameDto> games=[
        new (
            1,
            "Street Fighter",
            "Fighting",
            20.99m,
            new DateOnly(2020,01,01)
        ),
        new (
            2,
            "Soccer",
            "Sports",
            29.99m,
            new DateOnly(2000,01,01)
        ),
        new (
            3,
            "Need For Speed",
            "Racing",
            40.99m,
            new DateOnly(1999,01,20))

    ];

    public static RouteGroupBuilder MapGameEndpoints(this WebApplication app){


        var group=app.MapGroup("games").WithParameterValidation();


                
        // GET /games
        group.MapGet("games",()=>games);

        //find a game of specific id
        //GET /games/id
        group.MapGet("games/{id}",(int id)=>
        {
            GameDto? game=games.Find(game=> game.Id==id);

            return game is null? Results.NotFound():Results.Ok(game);
        
        }
        )
        .WithName(GetGameEndpoint);
        //.WithName("GetGame")--is used to allow resource finding of the dtos


        //GET /
        group.MapGet("/", () => "Hello World!");

        //POST /games
        group.MapPost("games",(CreateGameDto newGame)=>
        {
            GameDto game=new(
                games.Count+1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate);

            //add new game to the game record
            games.Add(game);

            //used to return resource to the map where resources can be accessed
            //Results.CreateAtRoute(resourcelocator,value--id ,payload)
            return Results.CreatedAtRoute(GetGameEndpoint,new {id=game.Id},game);
        }
        );

        //update
        //PUT /games 
        group.MapPut("games/{id}",(int id, UpdateGameDto updateGame) =>{
            var index=games.FindIndex(game=>game.Id==id);
            
            
            if (index==-1){
                return Results.NotFound();
            }
            else{
                games[index]=new GameDto(
                id,
                updateGame.Name,
                updateGame.Genre,
                updateGame.Price,
                updateGame.ReleaseDate
                );
                return Results.NoContent();
                }
                
        });

        group.MapDelete("games/{id}",(int id)=>{
            games.RemoveAll(game=>game.Id==id);
            return Results.NoContent();
        });

        


        return group;
    }

    
}
