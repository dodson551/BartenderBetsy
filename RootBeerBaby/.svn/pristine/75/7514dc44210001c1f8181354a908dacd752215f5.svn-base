Module modRating

  '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'
  'adds stuff to the listview in the rate_drink form, pretty simple
  '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~'

  Public Function Rate_Drink(ByRef lst As ListView, ByVal frm As Form)
    RatingDialog.Show()
    For Each i As ListViewItem In lst.Items
      Dim itm As ListViewItem = RatingDialog.lstViewInfo.Items.Add(i.SubItems(1).Text)
      itm.SubItems.Add(i.SubItems(2).Text)
      itm.SubItems.Add(i.Text)
    Next
    Return Nothing
  End Function

End Module
