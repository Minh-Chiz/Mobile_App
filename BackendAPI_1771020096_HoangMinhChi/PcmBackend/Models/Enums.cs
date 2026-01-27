namespace PcmBackend.Models
{
    public enum RankLevel
    {
        Standard = 0,
        Silver = 1,
        Gold = 2,
        Diamond = 3
    }

    public enum WalletTransactionType
    {
        Deposit,    // Nạp
        Withdraw,   // Rút
        Payment,    // Thanh toán phí
        Refund,     // Hoàn tiền
        Reward      // Thưởng giải
    }

    public enum WalletTransactionStatus
    {
        Pending,
        Completed,
        Rejected,
        Failed
    }

    public enum BookingStatus
    {
        PendingPayment,
        Confirmed,
        Cancelled,
        Completed
    }

    public enum TournamentFormat
    {
        RoundRobin, // Vòng tròn
        Knockout,   // Loại trực tiếp
        Hybrid
    }

    public enum TournamentStatus
    {
        Open,
        Registering,
        DrawCompleted,
        Ongoing,
        Finished
    }

    public enum MatchWinningSide
    {
        Team1,
        Team2
    }

    public enum MatchStatus
    {
        Scheduled,
        InProgress,
        Finished
    }

    public enum NotificationType
    {
        Info,
        Success,
        Warning
    }
}