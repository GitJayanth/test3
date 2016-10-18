Namespace HyperHelp

Public Module Navigation

  Public Function AddLink(ByVal txt As String) As LinkButton
    Dim link As New LinkButton
    link.BorderWidth = link.BorderWidth.Pixel(0)
    link.BackColor = Color.Transparent
    link.Text = txt

    Return link

  End Function

End Module

End Namespace
