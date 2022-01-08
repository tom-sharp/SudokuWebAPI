using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Sudoku.Puzzle;
using SudokuWebAPI.Dtos;

/*

	A web api to create or solve sudoku puzzles.
	(Dependent on the SudokuPuzzle assembly)

	http(s)://server/api/sudoku
		will generate a random sudoku puzzle and return it as a json object with:
			status = indicate that all went well and should be "Ok"
			puzzle = is the puzzle string
	
	http(s)://server/api/sudoku/{puzzle}
		will solve sudoku {puzzle} and return a json object with:
			status = indicate that all went well and should be "Ok" if solved
			puzzle = is the solved puzzle string

	ver		description
	0.02	Updated to use SudokuPuzzle 0.08 (NumPass)
	0.01	initial
 
 */



namespace SudokuWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SudokuController : ControllerBase
	{
		// GET: api/<SudokuController>
		[HttpGet]
		public ActionResult<PuzzleDto> Get() {
			return Ok(new PuzzleDto("Ok", new CreateSudoku().GetSudokuPuzzle().GetPuzzle()));
		}

		// GET api/<SudokuController>/5
		[HttpGet("{puzzle}")]
		public ActionResult<PuzzleDto> Get(string puzzle) {
			SudokuPuzzle sudoku = new SudokuPuzzle(puzzle);
			if (!sudoku.IsValid()) return Ok(new PuzzleDto("Invalid", sudoku.GetPuzzle()));
			sudoku.ResolveRules();
			if (!sudoku.IsValid()) return Ok(new PuzzleDto("Invalid", sudoku.GetPuzzle()));
			if (!sudoku.IsSolved()) sudoku.ResolveNumPass();
			if (sudoku.IsSolved()) return Ok(new PuzzleDto("Ok", sudoku.GetPuzzle()));
			return Ok(new PuzzleDto("Invalid", sudoku.GetPuzzle()));
		}

	}
}
