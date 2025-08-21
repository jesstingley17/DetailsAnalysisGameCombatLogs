
export type MenuItemModel = {
    id: number;
    label: string;
    navigateTo: string;
    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    icon: any;
    disabled: boolean;
    subMenu: MenuItemModel[] | null;
}