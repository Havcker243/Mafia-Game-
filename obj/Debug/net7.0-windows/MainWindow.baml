   M S B A M L   `   `   `  ����G  CMafiaManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:MafiaManagerMafiaManager  5   	       MafiaManager.MainWindow   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:MafiaManager     5   	   -   5          � Title$  
MainWindow��5   	   $	��500��6   $	��850��6)   .��5	      � 
���   �.+�5
   
   � $��215,24,215,70q�6   .+�5      �� -   5        $��Play Now��5      $��Center=�6(   $	��Top=�6E   $��	0,260,0,0q�6r   $��35��6�   $	��120��6�   $��14(�6�   �� 5      -   5         ��Name$ PlayerCountLabel��5      $��Number of players: ��6.   $��Center=�6L   $	��Top=�6i   $	��160��6�   $��14(�6�   $��	0,104,0,0q�6�   
�������6�   �� 5      -   5         ��Name$ PlayerCountSlider��5      $��Center=�60   $��	0,133,0,0q�6M   $	��Top=�6`   $	��350��5       J� RenderTransformOrigin$ 0.5,0.5&�6   $��37��6=    � Minimum$ 4Q�6I    � Maximum$ 12Q�6U    �� TickFrequency$ 1Q�6b    �� TickPlacement$ BottomRight=�5       �� IsSnapToTickEnabled$
 True��6-   6H   �� 5      -   5        	 ��Name$	 SheriffCheckBox��5      $��Sheriff��60   $��Center=�6B   $��160,180,0,0q�6_   $	��Top=�6t   
 d� 	IsChecked$

 TrueY�6�   
�������6�   �� 5      -   5        $	 DoctorCheckBox��5      $��Doctor��6/   $��Center=�6@   $��	0,180,0,0q�6]   $	��Top=�6p   $��60��6�   $

 TrueY�6�   
�������6�   �� 5      $��Center=�6   $��0,35,0,0q�65   $
��Wrap=�6G   $�� Malicious Mafia Manager��6[   $	��Top=�6{   $��70)�6�   $��20��6�   $	��210��6�   $��18(�6�    �� TextAlignment$ Center=�6�   
�������6�   �� 5      $��Center=�6   $��-140,181,0,0q�65   $
��Wrap=�6K   $��Roles:��6_   $	��Top=�6m   $��70)�6�   $ Center=�6�   
�������6�   5      �� 5   
   $��-26,206,548,-3q�6   $��/Images/StartPage.png��6(   $
��None=�6G   5      5      