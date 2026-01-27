using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // [Mới] Để dùng FirstOrDefaultAsync, AnyAsync
using PcmBackend.Data; // [Mới]
using PcmBackend.Models;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
public class TournamentsController : ControllerBase
{
    // [Sửa] Dùng AppDbContext
    private readonly AppDbContext _context;

    public TournamentsController(AppDbContext context)
    {
        _context = context;
    }

    // POST: Tham gia giải đấu
    [HttpPost("{id}/join")]
    [Authorize]
    public async Task<IActionResult> JoinTournament(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // [Sửa] _context.Members thay vì _context.Members_096
        var member = await _context.Members.FirstOrDefaultAsync(m => m.UserId == userId);

        // [Sửa] _context.Tournaments
        var tournament = await _context.Tournaments.FindAsync(id);

        if (tournament == null || member == null) return NotFound();

        // 1. Kiểm tra đã tham gia chưa
        // [Sửa] _context.TournamentParticipants
        bool joined = await _context.TournamentParticipants
            .AnyAsync(p => p.TournamentId == id && p.MemberId == member.Id);
        if (joined) return BadRequest("Bạn đã đăng ký giải này rồi.");

        // 2. Kiểm tra ví (Entry Fee)
        if (member.WalletBalance < tournament.EntryFee)
            return BadRequest("Số dư ví không đủ để đóng lệ phí.");

        // 3. Trừ tiền & Đăng ký (Transaction)
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            // Trừ tiền
            member.WalletBalance -= tournament.EntryFee;

            // Log giao dịch
            // [Sửa] WalletTransactions_096 (có 's'), _context.WalletTransactions
            _context.WalletTransactions.Add(new WalletTransactions_096
            {
                MemberId = member.Id,
                Amount = -tournament.EntryFee,
                // [Sửa] WalletTransactionType (Enum đúng)
                Type = WalletTransactionType.Payment,
                // [Sửa] WalletTransactionStatus (Enum đúng)
                Status = WalletTransactionStatus.Completed,
                Description = $"Phí tham gia giải {tournament.Name}",
                RelatedId = $"Tour_{id}"
            });

            // Thêm người tham gia
            // [Sửa] _context.TournamentParticipants
            _context.TournamentParticipants.Add(new TournamentParticipants_096
            {
                TournamentId = id,
                MemberId = member.Id,
                PaymentStatus = true
            });

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new { Message = "Đăng ký thành công!" });
        }
        catch
        {
            await transaction.RollbackAsync();
            return StatusCode(500);
        }
    }
}