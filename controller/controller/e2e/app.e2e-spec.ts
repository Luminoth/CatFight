import { ControllerPage } from './app.po';

describe('controller App', () => {
  let page: ControllerPage;

  beforeEach(() => {
    page = new ControllerPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!');
  });
});
