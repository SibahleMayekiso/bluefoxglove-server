﻿using BlueFoxGloveAPI.Models;
using BlueFoxGloveAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlueFoxGloveAPI.Controllers
{
    [ApiController]
    [Route("/")]
    public class GameController: ControllerBase
    {
        private readonly IGameRepository _gameRepository;

        public GameController(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        [HttpPost("[controller]/gamesession")]
        public async Task<IActionResult> CreateGameSession(Game game)
        {
            return null;
        }

        [HttpGet("[controller]/gamesession")]
        public async Task<IActionResult> GetAllGameSessions()
        {
            return null;
        }

        [HttpGet("[controller]/gamesession/{gameSessionId}")]
        public async Task<IActionResult> GetGameSessionById(string gameSessionId)
        {
            return null;
        }

        [HttpPut("[controller]/playerjointime/{gameSessionId}")]
        public async Task<IActionResult> UpdatePlayerJoinTime(Player player)
        {
            return null;
        }

        [HttpGet("[controller]/playerpostion/{playerID}")]
        public async Task<IActionResult> GetPlayerPosition(string playerID)
        {
            return null;
        }

        [HttpPut("[controller]/playerpostion")]
        public async Task<IActionResult> UpdatePlayerPosition(Player player)
        {
            return null;
        }

        [HttpGet("[controller]/playerhealth/{playerID}")]
        public async Task<IActionResult> GetPlayerHealth(string playerID)
        {
            return null;
        }

        [HttpPut("[controller]/playerhealth")]
        public async Task<IActionResult> UpdatePlayerHealth(Player player)
        {
            return null;
        }

        [HttpGet("[controller]/playerpoints/{playerID}")]
        public async Task<IActionResult> GetPlayerPoints(string playerID)
        {
            return null;
        }

        [HttpPut("[controller]/playerpoints")]
        public async Task<IActionResult> UpdatePlayerPoints(Player player)
        {
            return null;
        }
    }
}