using BVH.FB.Model;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Interactions;
using System.Threading;
using BVH.FB.Common;
using BVH.FB.Model.CustomException;
using System.IO;

namespace BVH.FB.Service
{
    public class FbPageCreator
    {
        IWebDriver _driver;
        AccountInfor _account;
        FormConfig _config;

        public FbPageCreator(AccountInfor account, FormConfig config)
        {
            _account = account;
            _config = config;

            var options = new ChromeOptions();
            options.AddUserProfilePreference("profile.default_content_setting_values.notifications", 2);
            options.AddArgument($@"user-data-dir=C:\ChromeProfiles\{account.UID}");
            options.AddArgument("--disable-cache");

            // Configure the ChromeDriver service
            var chromeDriverService = ChromeDriverService.CreateDefaultService("Assets/chromedriver.exe");
            chromeDriverService.SuppressInitialDiagnosticInformation = true;

            _driver = new ChromeDriver(chromeDriverService, options);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(_config.Wait);
        }

        public void Execute()
        {
            try
            {
                Login();
                CreatePage();
            }
            catch (Exception ex) 
            {
                _account.State = !String.IsNullOrEmpty(_account.State) ? _account.State : $"Error:{ex.Message}";
            }
            finally {
                _driver.Quit();
            }
        }

        private void Login()
        {
            try
            {
                _driver.Navigate().GoToUrl("https://mbasic.facebook.com");
                try
                {
                    var txtEmail = _driver.FindElement(By.Name("email"));
                    var txtPass = _driver.FindElement(By.Name("pass"));
                    var btnLogin = _driver.FindElement(By.Name("login"));

                    txtEmail.SendKeys(_account.UID);
                    txtPass.SendKeys(_account.Password);
                    btnLogin.Click();
                }
                catch (NoSuchElementException)
                {
                }

                if (_driver.Url.Contains("/checkpoint/1501092823525282"))
                {
                    _account.State = "Error: Checkpoint 282";
                    throw new CheckPoint282Exception();
                }

                if (_driver.Url.Contains("mbasic.facebook.com/checkpoint"))
                {
                    if(String.IsNullOrEmpty(_account.TwoFactor))
                    {
                        _account.State = "Error: Không có dữ liệu 2FA.";
                        throw new NotConfigTwoFactorException();
                    }

                    CheckPoint();
                }

                if (_driver.Url.Contains("mbasic.facebook.com/login/checkpoint"))
                {
                    SaveCheckPoint();
                }

                if (_driver.Url.Contains("save-device"))
                {
                    NotSaveBrowser();
                }

                if(_driver.Url.Contains("gettingstarted"))
                {
                    return;
                }

                if(_driver.Url.Contains("mbasic.facebook.com/login/?email="))
                {
                    _account.State = "Error: Sai tài khoản/mật khẩu";
                    throw new WrongUsernameAndPasswordException();
                }

                //try
                //{
                //    var btnPost = _driver.FindElement(By.Name("view_post"));
                //}
                //catch (Exception)
                //{
                //    _account.State = "Error: Không thể nhận diện main page";
                //    throw new UnrecognizeMainPageException();
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CheckPoint()
        {
            var txtTwoFactorCode = _driver.FindElement(By.Name("approvals_code"));
            txtTwoFactorCode.SendKeys(Utilities.Get2FACode(_account.TwoFactor.Replace(" ", "")));

            var btnSubmit2FA = _driver.FindElement(By.Name("submit[Submit Code]"));
            btnSubmit2FA.Click();
        }

        public void SaveCheckPoint()
        {
            var btnSubmit2FA = _driver.FindElement(By.Name("submit[Continue]"));
            btnSubmit2FA.Click();
        }

        public void NotSaveBrowser()
        {
            var btnNotNow = _driver.FindElement(By.XPath("//*[@id='root']/table/tbody/tr/td/div/div[3]/a"));
            btnNotNow.Click();
        }

        private void CreatePage()
        {
            _driver.Navigate().GoToUrl("https://www.facebook.com/pages/creation?ref_type=launch_point");
            int flow = 1;
            
            try
            {
                var txtPageName = _driver.FindElement(By.ClassName("x10emqs4"));
                var randomName = Utilities.RandomName();
                txtPageName.SendKeys(randomName);

                var txtPageCate = _driver.FindElement(By.ClassName("x1w0mnb"));
                txtPageCate.SendKeys("Nhà hàng");
                Thread.Sleep(2000);
                txtPageCate.SendKeys(Keys.ArrowDown);
                txtPageCate.SendKeys(Keys.Enter);

                txtPageCate.SendKeys("Restaurant");
                Thread.Sleep(2000);
                txtPageCate.SendKeys(Keys.ArrowDown);
                txtPageCate.SendKeys(Keys.Enter);

                var divCreatePage = _driver.FindElement(By.ClassName("x1geds6s"));

                try
                {
                    var btnCreatePage = divCreatePage.FindElement(By.XPath("div[1]/div/div"));
                    btnCreatePage.Click();
                }
                catch (Exception)
                {
                    // Nếu exception tức là rơi vào loại account thứ 2, loại mà tạo ko qua step
                    var btnCreatePage = divCreatePage.FindElement(By.XPath("div[2]/div/div"));
                    btnCreatePage.Click();
                    flow = 2;
                }
                
            }
            catch (Exception ex)
            {
                _account.State = $"Error: Set PageName and Cate: ${ex.Message}";
                throw new CreatePageException();
            }

           
            Thread.Sleep(20000);

            if(flow == 1)
            {
                IWebElement divStep;
                IWebElement lbStep;

                try
                {
                    divStep = _driver.FindElement(By.ClassName("x1f0eagk"));
                    lbStep = divStep.FindElement(By.XPath("div[1]/span"));
                }
                catch (Exception ex)
                {
                    _account.State = $"Error: Read step label: ${ex.Message}";
                    throw new CreatePageException();
                }

                if (lbStep.Text.Contains("1"))
                {
                    try
                    {
                        var divCreatePage = _driver.FindElement(By.ClassName("x1geds6s"));
                        var btnNextStep = divCreatePage.FindElement(By.XPath("div[2]/div[2]/div"));
                        btnNextStep.Click();
                        Thread.Sleep(5000);
                    }
                    catch (Exception ex)
                    {
                        _account.State = $"Error: Next step 1: ${ex.Message}";
                        throw new CreatePageException();
                    }
                }

                if (lbStep.Text.Contains("2"))
                {
                    try
                    {
                        var divUploadAvatar = _driver.FindElement(By.ClassName("xeq5yr9"));
                        var inputUploadAvatar = divUploadAvatar.FindElement(By.XPath("input"));
                        var randomAvatar = Utilities.RandomFile(Path.Combine(_config.ImagePath, "Avatar"));
                        inputUploadAvatar.SendKeys(randomAvatar);
                        Thread.Sleep(10000);
                    }
                    catch (Exception ex)
                    {
                        _account.State = $"Error: Upload Avatar: ${ex.Message}";
                        throw new CreatePageException();
                    }

                    try
                    {
                        var divUploadAvatar = _driver.FindElement(By.ClassName("xeq5yr9"));
                        var inputUploadCover = divUploadAvatar.FindElement(By.XPath("input"));
                        var randomCover = Utilities.RandomFile(Path.Combine(_config.ImagePath, "Cover"));
                        inputUploadCover.SendKeys(randomCover);
                        Thread.Sleep(10000);
                    }
                    catch (Exception ex)
                    {
                        _account.State = $"Error: Upload Cover: ${ex.Message}";
                        throw new CreatePageException();
                    }

                    try
                    {
                        var divCreatePage = _driver.FindElement(By.ClassName("x1geds6s"));
                        var btnNextStep = divCreatePage.FindElement(By.XPath("div[2]/div[2]/div"));
                        btnNextStep.Click();
                        Thread.Sleep(10000);
                    }
                    catch (Exception ex)
                    {
                        _account.State = $"Error: Next step 2: ${ex.Message}";
                        throw new CreatePageException();
                    }
                }


                if (lbStep.Text.Contains("3"))
                {
                    try
                    {
                        var divCreatePage = _driver.FindElement(By.ClassName("x1geds6s"));
                        var btnSkip = divCreatePage.FindElement(By.XPath("div[2]/div[2]/div"));
                        btnSkip.Click();
                        Thread.Sleep(5000);
                    }
                    catch (Exception ex)
                    {
                        _account.State = $"Error: Next step 3: ${ex.Message}";
                        throw new CreatePageException();
                    }
                }

                if (lbStep.Text.Contains("4"))
                {
                    try
                    {
                        var divCreatePage = _driver.FindElement(By.ClassName("x1geds6s"));
                        var btnNextStep = divCreatePage.FindElement(By.XPath("div[2]/div[2]/div"));
                        btnNextStep.Click();
                        Thread.Sleep(5000);
                    }
                    catch (Exception ex)
                    {
                        _account.State = $"Error: Next step 4: ${ex.Message}";
                        throw new CreatePageException();
                    }
                }

                if (lbStep.Text.Split('5').Length > 2)
                {
                    try
                    {
                        var divCreatePage = _driver.FindElement(By.ClassName("x1geds6s"));
                        var btnDone = divCreatePage.FindElement(By.XPath("div[2]/div[2]/div"));
                        btnDone.Click();
                        Thread.Sleep(25000);
                    }
                    catch (Exception ex)
                    {
                        _account.State = $"Error: Next step 5: ${ex.Message}";
                        throw new CreatePageException();
                    }

                }

                try
                {
                    var finalUrl = _driver.Url;
                    string[] urlSplits = finalUrl.Split(new[] { "id=" }, StringSplitOptions.None);
                    _account.PageID = urlSplits[1];
                    _account.State = "Success: Tạo page thành công";
                }
                catch (Exception ex)
                {
                    _account.State = $"Error: Get PageID: ${ex.Message}";
                    throw new CreatePageException();
                }
            }
            else
            {
                try
                {
                    var divUploadAvatar = _driver.FindElement(By.ClassName("xeq5yr9"));
                    var inputUploadAvatar = divUploadAvatar.FindElement(By.XPath("input"));
                    var randomAvatar = Utilities.RandomFile(Path.Combine(_config.ImagePath, "Avatar"));
                    inputUploadAvatar.SendKeys(randomAvatar);
                    Thread.Sleep(10000);
                }
                catch (Exception ex)
                {
                    _account.State = $"Error: Upload Avatar: ${ex.Message}";
                    throw new CreatePageException();
                }

                try
                {
                    var divUploadAvatar = _driver.FindElement(By.ClassName("xeq5yr9"));
                    var inputUploadCover = divUploadAvatar.FindElement(By.XPath("input"));
                    var randomCover = Utilities.RandomFile(Path.Combine(_config.ImagePath, "Cover"));
                    inputUploadCover.SendKeys(randomCover);
                    Thread.Sleep(10000);
                }
                catch (Exception ex)
                {
                    _account.State = $"Error: Upload Cover: ${ex.Message}";
                    throw new CreatePageException();
                }

                try
                {
                    var divCreatePage = _driver.FindElement(By.ClassName("x1geds6s"));
                    var btnNextStep = divCreatePage.FindElement(By.XPath("div[1]/div/div"));
                    btnNextStep.Click();
                    Thread.Sleep(10000);
                }
                catch (Exception ex)
                {
                    _account.State = $"Error: Next step 2: ${ex.Message}";
                    throw new CreatePageException();
                }

                try
                {
                    var finalUrl = _driver.Url;
                    string[] urlSplits = finalUrl.Split(new[] { "/" }, StringSplitOptions.None);
                    _account.PageID = urlSplits[1];
                    _account.State = "Success: Tạo page thành công";
                }
                catch (Exception ex)
                {
                    _account.State = $"Error: Get PageID: ${ex.Message}";
                    throw new CreatePageException();
                }
            }
            _account.Cookie = String.Join(";", _driver.Manage().Cookies.AllCookies.Select(_ => $"{_.Name}={_.Value}"));
        }

        private void UploadAvatarAndCover()
        {

        }
    }
}
