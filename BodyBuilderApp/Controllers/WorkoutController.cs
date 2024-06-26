﻿using BodyBuilder.Application.Dtos.Movement;
using BodyBuilder.Application.Dtos.Workout;
using BodyBuilder.Application.Dtos.WorkoutMovement;
using BodyBuilder.Application.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BodyBuilderApp.Controllers {
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WorkoutController : ControllerBase {
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService) {
            _workoutService = workoutService;
        }

        [HttpPost]
        public async Task<IActionResult> StartWorkout(WorkoutAddDto workoutAddDto) {
            TimeSpan _startTime;
            TimeSpan.TryParse(workoutAddDto.StartTime2, out _startTime);
            workoutAddDto.StartTime = _startTime;
            return Ok(await _workoutService.StartWorkoutAsync(workoutAddDto));
        }

        [HttpPut("{workoutId}")]
        public async Task<IActionResult> FinishWorkout([FromRoute] Guid workoutId,string endTime,int duration) {
            TimeSpan _endTime;
            TimeSpan.TryParse(endTime, out _endTime);
            return Ok(await _workoutService.FinishWorkout(workoutId, _endTime,duration));

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id) {
            return Ok(await _workoutService.GetWorkoutByIdAsync(id));
        }

        [HttpGet("{userid}")]
        public async Task<IActionResult> GetByUserId(Guid userid) {
            return Ok(await _workoutService.GetWorkoutByUserIdAsync(userid));
        }

        [HttpGet("{subprogrammeid}")]
        public async Task<IActionResult> GetBySubprogrammeId(Guid subprogrammeid) {
            return Ok(await _workoutService.GetWorkoutBySubprogrammeIdAsync(subprogrammeid));
        }

        [HttpPost("{workoutId}")]
        public async Task<IActionResult> SaveWorkoutMovements([FromRoute] Guid workoutId, [FromBody] List<WorkoutMovementAddDto> movementAddDto) {
            return Ok(await _workoutService.CreateWorkoutMovement(workoutId, movementAddDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout([FromRoute] Guid id) {
            return Ok(await _workoutService.StopWorkout(id));
        }
    }
}