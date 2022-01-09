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
			status = return status should be "ok"
			puzzle = is the puzzle string
	
	http(s)://server/api/sudoku/{puzzle}
		will verify an already solved puzzle or solve a sudoku puzzle challange and return a json object with:
			status = will be "invalid" if puzzle is invalid or incorrecty solved. will be "solved" if {puzzle} is solved correctly or "ok" if {puzzle} challange was solved
			puzzle = is the solved puzzle string or the invalid puzzle

	ver		description
	0.03	Updated return object status code {Invalid, Solved, Ok} to support validation of a solved puzzle
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
			return Ok(new PuzzleDto(statusOk, new CreateSudoku().GetSudokuPuzzle().GetPuzzle()));
		}

		// GET api/<SudokuController>/5
		[HttpGet("{puzzle}")]
		public ActionResult<PuzzleDto> Get(string puzzle) {

			SudokuPuzzle sudoku = new SudokuPuzzle(puzzle);
			if (!sudoku.IsValid()) return Ok(new PuzzleDto(statusInvalid, sudoku.GetPuzzle()));
			if (sudoku.IsSolved()) return Ok(new PuzzleDto(statusSolved, sudoku.GetPuzzle()));

			sudoku.ResolveNumPass();
			if (sudoku.IsSolved()) return Ok(new PuzzleDto(statusOk, sudoku.GetPuzzle()));
			return Ok(new PuzzleDto(statusInvalid, sudoku.GetPuzzle()));

		}

		string statusOk = "Ok";
		string statusSolved = "Solved";
		string statusInvalid = "Invalid";

	}
}
