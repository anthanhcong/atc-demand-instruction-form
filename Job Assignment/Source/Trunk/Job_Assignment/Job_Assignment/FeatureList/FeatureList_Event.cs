using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MasterDatabase;


namespace Job_Assignment
{
    public partial class Form1 : SQL_APPL
    {
        private int SkillList_Index = 0;
        private int Line_DesciptionList_Index = 2;
        private int LineSkillRequestList_Index = 3;
        private int InputFromPlannerList_Index = 4;
        private int ProductionPlanByDate_Index = 5;
        private int ProductionPlanByWorkStation_Index = 6;
        private int WorkStationDescription_Index = 7;
        private int Tracking_Index = 7;
        private int Leave_Info_Index = 9;
        private int Employee_Working_on_Sunday_Index = 10;
        const string TAB_TRACKING = "Tracking";

        /*************************************************************
                   ##   ##    ##   ###### ####### # #### ######       
                   ##   ##   # ##  ##        #    #      #    #       
                   # # # #  ##  #    ####    #    # ###  ######       
                   # ### #  ###### #    ##   #    #      #    #       
                   #  #  # #     #  #####    #    ###### #    #       
                                                                      
                                                                      
            #####     ##   #######   #    #####    ##     ####  ######
            #   ##   ###      #     ###   #   ##   ###   #   ## #     
            #    ##  #  #    ##    ## ##  #####   #  ##  #####  ######
            #    #  ######   ##    #####  #    #  #####       # #     
            # #### ##    #   ##   #     # ###### #    ## ###### ######

         **************************************************************/

        private void ListProductionLine_Link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SkillList_Init();
        }

        private void lbl_Empl_Skill_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Empl_Skill_List_Init();
        }

        private void lbl_LineDescription_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Line_DesciptionList_Init();
        }

        private void lbl_LineSkillRequest_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LineSkillRequestList_Init();
        }

        private void MDB6_ShiftDescription_Lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShiftDescription_Init();
        }

        private void MDB007_FixPositionTable_Lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FixPosition_Init();
        }

        private void MDB008_SpecicalLine_Tbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SpecialLine_Init();
        }

        private void MDB009_LineLayout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LineLayout_Init();
        }

        private void lbl_MDB10_LeaveInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Leave_Info_Init();
        }

        private void MDB_011_Employee_Working_on_Sunday_linklbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Employee_Working_on_Sunday_Init();
        }

        /****************************************************************
            ######  ######   #####    ######  ####### ######  ##### 
            #    #  #    #  #     ## ##     # #       #      ##     
            ######  ######  #      # #        ######   #####   #### 
            #       #    #  #     ## ##     # #      #     # #     #
            #       #    #   ######   ######  ######  ######  ######

         ****************************************************************/

        private void lbl_InputFromPlanner_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            P001_InputFromPlanner_Init();
        }
        private void lbl_ProductionPlanByDate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            P002_PlanByDate_Init();
        }

        private void P_004_CreateWST_Plan_link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            P004_ProductionPlanDetail_Init();
        }

        private void lbl_ProductionPlanByWorkStation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            P004_ProductionPlanDetail_Init();
        }
        private void lbl_WorkStationDescription_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LineWorkStationMapping_Init();
        }
        private void llbTracking_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!tabControl1.TabPages.ContainsKey(TAB_TRACKING))
            {
                P007_P008_ucTracking uc = new P007_P008_ucTracking(ProgressBar1, StatusLabel1, StatusLabel2);
                uc.Dock = DockStyle.Fill;
                TabPage page = new TabPage(TAB_TRACKING);
                page.Name = TAB_TRACKING;
                page.Controls.Add(uc);
                tabControl1.TabPages.Add(page);
               // tabControl1.TabPages.Insert(Tracking_Index, page);
            }
            tabControl1.SelectedTab = tabControl1.TabPages[TAB_TRACKING];
        }

        private void FeatureList_TrackingView_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            P007_Tracking_View_Init();
        }

        private void FeatureList_R002_Skill_Mapping_LinkLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Skill_Mapping_Init();
        }

        private void KittingDatabase_Lnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            KittingDatabase_Init();
        }


        private void P_003_Working_Arrange_link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            P003_AssignEmpl_Init();
        }

        private void EmplWorkingPlan_LinkLB_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            P005_EmplWorkingPlan_Init();
        }

        private void R004_TrackingKHTT_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            R004_Tracking_KHTT_Init();
        }

        private void R_005_Employee_Review_linklbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            R_005_Employee_Review_Init();
        }

        private void R_006_TrackingTT_PlanTL_linklbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            R_006_TrackingTT_PlanTL_Init();
        }

        private void R_007_Emlpoyee_In_WST_linklbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            R_007_Emlpoyee_In_WST_Init();
        }

        private void R_008_EmployeeCurrentLinklbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            R_008_Employee_Current_On_Line_Init();
        }

        private void R_009_Line_Status_linklbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            R_009_Line_Status_Init();
        }

        private void CreatePlanforDateToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            DateTime date;
            DateSelect_Dialog_Form selectDate_Dialog = new DateSelect_Dialog_Form(DateTime.Now.AddDays(1));
            if (selectDate_Dialog.ShowDialog() == DialogResult.OK)
            {
                date = selectDate_Dialog.Select_Date;
                Create_All_Plan_for_Date(date);
            }
        }

        private bool Create_All_Plan_for_Date(DateTime date)
        {
            StatusLabel1.Visible = true;
            ProgressBar1.Visible = true;

            // Copy Plan From Kitting
            StatusLabel1.Text = "Get Plan From Kitting";
            P001_InputFromPlanner_Init();
            Coppy_Plan_From_Kitting(date);

            // Create Plan for Date
            P002_PlanByDate_Init();
            Create_PlanByDate(date);

            // Assign Empl
            P003_AssignEmpl_Init();
            Create_Plan_For_Line(date);

            // Create Plan in Detail
            P004_ProductionPlanDetail_Init();
            Create_Details_Plan(date);

            // Create Plan for Empl
            P005_EmplWorkingPlan_Init();
            Create_Empl_Plan(date);

            StatusLabel1.Visible = false;
            ProgressBar1.Visible = false;

            return true;
        }
    }
}
