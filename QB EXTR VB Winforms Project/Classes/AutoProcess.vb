Public Class AutoProcess
    Private _Name As String
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Private _ScheduleTime As String

    Public Property ScheduleTime() As String
        Get
            Return _ScheduleTime
        End Get
        Set(ByVal value As String)
            _ScheduleTime = value
        End Set
    End Property







End Class
