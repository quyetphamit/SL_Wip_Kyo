   M S B A M L   `   `   `  ����H  DKIO-BOARD LED, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:STM_PDA_System.MasterInputSTM_PDA_System.MasterInput  5   	   (    "STM_PDA_System.MasterInput.wMaster   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :local(clr-namespace:STM_PDA_System.MasterInput     5   	     � Title$  wMaster��5   	   $	��500��6   $
��1000��6&    � 	Resources 5	       �� MergedDictionaries �� Source�� 5
   
    5      �� 5      $ ButtonControl.xaml?�6%   %    5      .��5      �  � RowDefinitions 5   
   �� 5      $��50 �6   �� 5      .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��10,0,0,0q�6.   .+�5      �� $��Chọn dữ liệu  ��6   $��Center=�6.   $��20(�6I   $
��Bold"�6W   �� 5      -   5        $��
cbTypeData��5      $	��200��6)   $��40��65    �� DisplayMemberPath$ display��6A    �� SelectedValuePath$ values��6]   $��20(�5      �� 5      -   5        $��btUpdate��5      $��Sửa DL��6%   $��40��66   $	��120��6B   $��20,0,0,0q�6N   $��20(�6`      buttonCustorm#���   6n   5      &   System.Windows.Controls.DataGrid  5   
   -   5        $��dgUpdate��5   4   $��1��6   $��
10,0,10,10q�6    $��16(�6D   5      5      