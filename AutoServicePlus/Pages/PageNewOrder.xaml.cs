﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoServicePlus.Pages;


public partial class PageNewOrder : UserControl {

	private bool isOrdEdit = false;
	private bool isClearMoved = false;

	public PageNewOrder() {
        InitializeComponent();
		Data.PropertiesChange.WinHeight = 343;
	}

	private void b_Edit_Click(object sender, RoutedEventArgs e) {
			Task.Factory.StartNew(() => {
				int hetbl = 0;
				this.Dispatcher.Invoke(() => { hetbl = Convert.ToInt32(this.dg_Заказ.ActualHeight); });

				if (isOrdEdit) {
					isOrdEdit = false;
					int he = 90;
					int he2 = 105;
					this.Dispatcher.Invoke(() => { this.nud_NumЗаказ.Visibility = Visibility.Hidden; });
					Task.Factory.StartNew(() => {
						while (he > 49) {
							this.Dispatcher.Invoke(() => { this.b_Del.Margin = new(0, he, 8, 0); });
							Thread.Sleep(7);
							he--;
						}
					});
					if (isClearMoved) {
						isClearMoved = false;
						Task.Factory.StartNew(() => {
							while (he2 > 7) {
								this.Dispatcher.Invoke(() => { this.b_Clear.Margin = new(0, 0, he2, 5); });
								Thread.Sleep(3);
								he2--;
							}
						});
					}
				} else {
					isOrdEdit = true;
					int he = 50;
					int he2 = 8;
					Task.Factory.StartNew(() => {
						while (he < 91) {
							this.Dispatcher.Invoke(() => { this.b_Del.Margin = new(0, he, 8, 0); });
							Thread.Sleep(7);
							he++;
						}
						this.Dispatcher.Invoke(() => { this.nud_NumЗаказ.Visibility = Visibility.Visible; });
					});
					if (hetbl < 152) {
						isClearMoved = true;
						Task.Factory.StartNew(() => {
							while (he2 < 106) {
								this.Dispatcher.Invoke(() => { this.b_Clear.Margin = new(0, 0, he2, 5); });
								Thread.Sleep(3);
								he2++;
							}
						});
					}
				}
			});
		}
	}
