using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuWebAPI.Dtos
{
	public class PuzzleDto
	{
		public PuzzleDto(string status, string puzzle) { this.Status = status; this.Puzzle = puzzle; }
		public string Status { get; set; }
		public string Puzzle { get; set; }
	}

}
